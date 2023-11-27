using UnityEngine;
using static PlayerInputConfig;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playMateLayer;
    [SerializeField] private Player player;

    private float horizontalInput;

    public enum PlayerState
    {
        Idle,
        Running,
        Sitting,
        Jumping,
        Carrying,
    }

    public enum PlayerDirection
    {
        Left,
        Right
    }

    private PlayerState state;
    private PlayerDirection direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInput = new PlayerInputConfig(player);
    }

    private void Update()
    {
        horizontalInput = 0f;

        // Cõng bạn
        if (bottomPlayer())
            setPlayerState(PlayerState.Carrying);
        else
            setPlayerState(PlayerState.Idle);

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
    
        // Cập nhật Animation theo trạng thái của player
        updateAnimation();
    }

    private void Move()
    {
        if (Input.GetKey(playerInput.moveLeft))
        {
            horizontalInput = -1f;
            setPlayerDirection(PlayerDirection.Left);
        }
        else if (Input.GetKey(playerInput.moveRight))
        {
            horizontalInput = 1f;
            setPlayerDirection(PlayerDirection.Right);
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

        // Xử lý quay mặt
        if(direction == PlayerDirection.Left)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = Vector3.one;

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
    public bool isGrounded()
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

    private void setPlayerDirection(PlayerDirection direction)
    {
        this.direction = direction;
    }

    public bool isFacingLeft()
    {
        return direction == PlayerDirection.Left;
    }

    public bool isFacingRight()
    {
        return direction == PlayerDirection.Right;
    }

    public bool isLeftPlayer(PlayerController otherPlayer) 
    {
        return Mathf.Sign(transform.position.x - otherPlayer.transform.position.x) == -1.0f;
    }

    public bool isRightPlayer(PlayerController otherPlayer)
    {
        return Mathf.Sign(transform.position.x - otherPlayer.transform.position.x) == 1.0f;
    }

    // Đang hướng về người chơi khác:
    // 1.Đứng bên trái người chơi đó và hướng mặt về bên phải
    // 2. Đứng bên phải người chơi đó và hướng mặt về bên trái
    public bool isFacingPlayer(PlayerController otherPlayer)
    {
        return (isLeftPlayer(otherPlayer) && isFacingRight()) || (isRightPlayer(otherPlayer) && isFacingLeft());
    }
}
