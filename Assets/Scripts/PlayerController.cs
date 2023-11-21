using UnityEngine;
using static PlayerInputConfig;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;
    private PlayerController playMate;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playMateLayer;
    [SerializeField] Player player;

    private float horizontalInput;

    private enum PlayerState
    {
        Idle,
        Running,
        Sitting,
        Jumping,
        Carrying,
    }

    private PlayerState state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInput = new PlayerInputConfig(player);

        GameObject playMateGameObject = (player == Player.Player1) ? GameObject.Find("Player 2") : GameObject.Find("Player 1");
        if (playMateGameObject != null)
            playMate = playMateGameObject.GetComponent<PlayerController>();
        else
            Debug.Log("playMateGameObject is null");
    }

    private void Update()
    {
        horizontalInput = 0f;

        // Các hành động chỉ có thể thực hiện nếu đang không ngồi và không cõng bạn
        if (state != PlayerState.Sitting && state != PlayerState.Carrying)
        {
            setPlayerState(PlayerState.Idle); // reset lại trạng thái idle -> tránh bị loop trạng thái (Running, Jumping) khi không có sự kiện chuyển trạng thái khác)

            // Di chuyển nhân vật theo chiều ngang
            if (Input.GetKey(playerInput.moveLeft) || Input.GetKey(playerInput.moveRight))
            {
                Move();
                setPlayerState(PlayerState.Running);
            }

            // Có thể nhảy khi đang tiếp đất và không có playMate trên đầu
            if (Input.GetKey(playerInput.moveUp))
                if (isGrounded())
                {
                    Jump();
                    setPlayerState(PlayerState.Jumping);
                }
        }

        // Ngồi
        if (Input.GetKeyDown(playerInput.moveDown) && isGrounded())
                setPlayerState(PlayerState.Sitting);
        if (Input.GetKeyUp(playerInput.moveDown))
            setPlayerState(PlayerState.Idle);

        // Cõng bạn
        if (bottomPlayer())
            setPlayerState(PlayerState.Carrying);
        else
            setPlayerState(PlayerState.Idle);

        // Cập nhật Animation theo trạng thái của player
        updateAnimation();
    }

    private void Move()
    {
        if (Input.GetKey(playerInput.moveLeft))
        {
            horizontalInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);       // flip
        }
        else if (Input.GetKey(playerInput.moveRight))
        {
            horizontalInput = 1f;
            transform.localScale = Vector3.one;                 // flip
        }

        Vector2 movement = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    private void updateAnimation()
    {

        anim.SetBool("grounded", isGrounded());
        anim.SetBool("run", isRunning());
        anim.SetBool("sit", isSitting() || bottomPlayer());

        if (isJumping())
            anim.SetTrigger("jump");
    }

    private void setPlayerState(PlayerState state)
    {
        this.state = state;
    }

    private bool isRunning()
    {
        return state == PlayerState.Running;
    }

    private bool isSitting()
    {
        return state == PlayerState.Sitting;
    }

    private bool isJumping()
    {
        return state == PlayerState.Jumping;
    }

    // Player tiếp đất khi đứng trên mặt đất hoặc đứng trên player khác
    private bool isGrounded()
    {
        RaycastHit2D groundLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D playMateLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, playMateLayer);

        return groundLayerRaycastHit.collider != null || playMateLayerRaycastHit.collider != null;
    }

    // Bị player khác leo lên đầu
    private bool bottomPlayer()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, playMateLayer);
        return raycastHit.collider != null;
    }
}
