using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public float HP = 50;
    public int ATK = 10;
    public float SPEED = 4;
    public float DEF = 0;

    [Header("Range Acttack")]
    [SerializeField] private Vector3 attackOffset;
    public float attackRange;
    [SerializeField] private LayerMask attackMask;

    [Header("Difference")]
    public bool isInvulnerable = false;
    public GameObject deathEffect;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 originPosition;

    private bool isStart = false;
    private bool grounded = false;
    private bool isLookAtRight = true;

    private float currentHealth;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = HP;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isStart = false;

        originPosition = transform.position;
    }

    public void Update()
    {
        if (PlayerManager.Instance.isDead())
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            currentHealth = HP;
            isStart = false;
            anim.SetBool("isStart", isStart);

            transform.position = originPosition;
            return;
        }

        if(!isStart && PlayerManager.Instance.Stage == PlayerManager.PlayerStage.Fight)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.down * 0.1f, ForceMode2D.Impulse);

            isStart = true;
            anim.SetBool("isStart", isStart);
        }
    }

    public void LookAtPlayer()
    {
        Transform player1 = PlayerManager.Instance.player1.transform;
        Transform player2 = PlayerManager.Instance.player2.transform;
        Transform player;

        float distance1 = Vector2.Distance(player1.position, transform.position);
        float distance2 = Vector2.Distance(player2.position, transform.position);

        if (distance1 <= distance2)
            player = player1;
        else
            player = player2;

        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        // Đứng bên phải player và nhìn vào bên phải
        if (transform.position.x > player.position.x && isLookAtRight)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isLookAtRight = false;
        } 
        // Đứng bên trái player và nhìn vào bên trái
        else if (transform.position.x < player.position.x && !isLookAtRight)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isLookAtRight = true;
        }
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK);
        }
    }

    public void AngryAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK * 2);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        float lossHealth = (1 - DEF/ 10.0f)  * damage;
        currentHealth -= lossHealth;

        if(currentHealth < HP * 0.3)
        {
            anim.SetBool("isAngry", true);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        StartCoroutine(DeathEffect());
    }

    private IEnumerator DeathEffect()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        PlayerManager.Instance.Win();
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
}
