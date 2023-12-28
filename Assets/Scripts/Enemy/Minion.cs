using System;
using System.Collections;
using UnityEngine;
public class Minion : MonoBehaviour
{
    [Header("Stats")]
    public float HP = 2;
    public float speed = 2f; // Tốc độ di chuyển

    [Header("Movement")]
    public Transform leftEdge;
    public Transform rightEdge;
    public Transform topEdge;
    public Transform bottomEdge;

    [Header("Attack")]
    public Vector3 attackOffset;
    public float attackHeight;
    public float attackWidth;
    public LayerMask attackMask;

    [Header("Difference")]
    public MinionHealthBar healthBar;
    public GameObject rewardContainer;

    private int direction = 1; // 1: right, -1: left    
    private Transform player1, player2;
    private bool grounded = false;

    //private bool canAttack = true; 
    public bool isAttack = false;
    private bool dead = false;


    private float currentHealth;
    public float health
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
            }
        }
    }

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player1 = PlayerManager.Instance.player1.transform;
        player2 = PlayerManager.Instance.player2.transform;

        health = HP;
        healthBar.setMaxHealth(HP);

        rewardContainer.SetActive(false);
    }

    public void Move()
    {
        Vector2 position = transform.position;

        if(position.x <= leftEdge.position.x || position.x >= rightEdge.position.x)
        {
            flip();
        }
        position.x += direction * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    private void flip()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        direction *= -1;

        transform.localScale = flipped;
        transform.Rotate(0f, 180f, 0f);
    }

    public void FollowPlayer1()
    {
        // nếu player đang bên trái mà đang đi về bên phải hoặc nếu player đang bên phải mà đi về bên trái
        if ((player1.position.x < transform.position.x && direction == 1) || (player1.position.x > transform.position.x && direction == -1))
            flip();

        if(!isAttack)
        {
            Vector2 position = transform.position;
            position.x += direction * speed * Time.deltaTime;
            rb.MovePosition(position);
        }
    }

    public void FollowPlayer2()
    {
        // nếu player đang bên trái mà đang đi về bên phải hoặc nếu player đang bên phải mà đi về bên trái
        if ((player2.position.x < transform.position.x && direction == 1) || (player2.position.x > transform.position.x && direction == -1))
            flip();

        if(!isAttack)
        {
            Vector2 position = transform.position;
            position.x += direction * speed * Time.deltaTime;
            rb.MovePosition(position);
        }
    }

    public bool IsPlayer1InRange()
    {
        return (player1.position.x >= leftEdge.position.x 
            && player1.position.x <= rightEdge.position.x 
            && player1.position.y >= bottomEdge.position.y
            && player1.position.y <= topEdge.position.y);
    }

    public bool IsPlayer2InRange()
    {
        return (player2.position.x >= leftEdge.position.x 
            && player2.position.x <= rightEdge.position.x
            && player2.position.y >= bottomEdge.position.y
            && player2.position.y <= topEdge.position.y);
    }

    void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapBox(pos, new Vector3(attackWidth, attackHeight), 0f, attackMask);

        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().Hurt();
        }
    }

    public void TakeDamage(int damage)
    {
        if (dead)
            return;

        health = Mathf.Clamp(health - damage, 0, HP);
        healthBar.setHealth(health);
        anim.SetTrigger("Hurt");

        if (health <= 0)
        {
            dead = true;
            anim.SetTrigger("Die");
            StartCoroutine(DeathEffect());
        }
    }

    private IEnumerator DeathEffect()
    {
        yield return new WaitForSeconds(1f);
        rewardContainer.SetActive(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;

            if (!grounded)
                anim.SetBool("grounded", true);

            grounded = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Gizmos.DrawWireCube(pos, new Vector3(attackWidth, attackHeight, 1));
    }
}
