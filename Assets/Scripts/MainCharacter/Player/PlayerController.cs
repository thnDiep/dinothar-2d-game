using UnityEngine;
using static PlayerInputConfig;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;
    [SerializeField] private PhysicsMaterial2D maxFriction;
    [SerializeField] private PhysicsMaterial2D highFriction;
    [SerializeField] private PhysicsMaterial2D normalFriction;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float rotateForce = 2.0f;
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

        setPlayerDirection(PlayerDirection.Right);
    }

    private void Update()
    {
        horizontalInput = 0f;

        // Cõng bạn
        if (bottomPlayer())
            setPlayerState(PlayerState.Carrying);
        else if (isCarrying() && !bottomPlayer())
            setPlayerState(PlayerState.Idle);

        // Ngồi
        if (Input.GetKeyDown(playerInput.moveDown) && isGrounded())
            setPlayerState(PlayerState.Sitting);

        if (isSitting() && Input.GetKeyUp(playerInput.moveDown))
            setPlayerState(PlayerState.Idle);

        if (!isSitting() && !isCarrying())
        {
            setPlayerState(PlayerState.Idle);

            // Chạy
            if (Input.GetKey(playerInput.moveLeft) || Input.GetKey(playerInput.moveRight))
            {
                Move();
                setPlayerState(PlayerState.Running);
            }

            // Nhảy
            if (Input.GetKey(playerInput.moveUp) && isGrounded())
            {
                Jump();
                setPlayerState(PlayerState.Jumping);
            }
        }

        
        updateAnimation();  // Cập nhật Animation theo trạng thái của player
        updateFriction();   // Cập nhật ma sát theo trạng thái của player
        updateConstraint();
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
        //if (PlayerManager.Instance.GetGameState() == PlayerManager.State.Normal)
        //rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        //else if (PlayerManager.Instance.GetGameState() == PlayerManager.State.Rotate)
        rb.AddForce(new Vector2(horizontalInput, 0) * rotateForce);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    private void updateAnimation()
    {

        anim.SetBool("grounded", isGrounded());
        anim.SetBool("run", isRunning());
        anim.SetBool("sit", isSitting() || isCarrying());

        if (isJumping())
            anim.SetTrigger("jump");

        // Xử lý quay mặt
        if(direction == PlayerDirection.Left)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = Vector3.one;
    }

    public void updateFriction()
    {
        if (state == PlayerState.Sitting || state == PlayerState.Carrying)
            boxCollider.sharedMaterial = maxFriction;
        else if (state == PlayerState.Idle)
            boxCollider.sharedMaterial = highFriction;
        else
            boxCollider.sharedMaterial = normalFriction;
    }

    public void updateConstraint()
    {
        if(state == PlayerState.Sitting) 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void setPlayerState(PlayerState state)
    {
        this.state = state;
    }

    public bool isRunning()
    {
        return state == PlayerState.Running;
    }

    public bool isSitting()
    {
        return state == PlayerState.Sitting;
    }

    public bool isJumping()
    {
        return state == PlayerState.Jumping;
    }

    public bool isCarrying()
    {
        return state == PlayerState.Carrying;
    }

    // Player tiếp đất khi đứng trên mặt đất hoặc đứng trên player khác
    public bool isGrounded()
    {
        RaycastHit2D groundLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D playMateLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, playMateLayer);

        return groundLayerRaycastHit.collider != null || playMateLayerRaycastHit.collider != null;
    }

    // Bị player khác leo lên đầu
    public bool bottomPlayer()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, playMateLayer);
        return raycastHit.collider != null;
    }

    private void setPlayerDirection(PlayerDirection direction)
    {
        this.direction = direction;
    }

    // Đang hướng về người chơi khác:
    // 1. Người chơi khác đứng bên phải và chạy về bên phải
    // 2. Người chơi khác đứng bên trái và chạy về bên trái
    public bool isRunToPlayer(PlayerController otherPlayer)
    {
        // xác định vị trí người chơi khác so với người chơi hiện tại
        float positionSign = Mathf.Sign(otherPlayer.transform.position.x - transform.position.x); // -1: left, 1: right
        return (positionSign == horizontalInput);
    }
}
