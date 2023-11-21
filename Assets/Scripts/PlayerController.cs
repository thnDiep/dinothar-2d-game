using UnityEngine;
using static PlayerInputConfig;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerInputConfig playerInput;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask otherPlayerLayer;
    [SerializeField] Player player;
    private float horizontalInput;

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

        // Di chuyển nhân vật theo chiều ngang
        if(Input.GetKey(playerInput.moveLeft) || Input.GetKey(playerInput.moveRight))
            Move();
       
        // Nhảy
        if (Input.GetKey(playerInput.moveUp))
            if(isGrounded() || onPlayer()) 
                Jump();

        // Cập nhật Animation
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
        anim.SetTrigger("jump");
    }

    private void updateAnimation()
    {
        anim.SetBool("grounded", isGrounded());

        anim.SetBool("run", horizontalInput != 0f);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onPlayer()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, otherPlayerLayer);
        return raycastHit.collider != null;
    }
}
