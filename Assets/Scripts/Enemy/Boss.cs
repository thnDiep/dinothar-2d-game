using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Boss : MonoBehaviour
{
    public static event Action<float> HealthChangeEvent;

    [Header("Stats")]
    public float HP = 50;
    public int ATK = 10;
    public float SPEED = 4;
    public float DEF = 0;

    [Header("Melee Attack")]
    public Vector3 meleeAttackOffset;
    public float meleeAttackRange;
    public LayerMask attackMask;

    [Header("Ranged Attack")]
    public bool hasRangedAttack = false;
    public Vector3 rangedAttackOffset;
    public float rangedAttackWidth;
    public float rangedAttackHeight;

    [Header("Difference")]
    public bool isInvulnerable = false;
    public GameObject deathEffect;
    public float deathEffectTimer = 1f;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 originPosition;

    private bool isStart = false;
    private bool grounded = false;
    private bool isLookAtRight = true;
    public bool isAttack = false;
    public bool isShoting = false;

    private float currentHealth;
    public float health
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                HealthChangeEvent?.Invoke(currentHealth);
            }
        }
    }

    public void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Start()
    {
        isStart = false;
        health = HP;
        UIInGame.Instance.bossHealthBar.setMaxHealth(HP);
        originPosition = transform.position;
    }

    public void Update()
    {
        if (PlayerManager.Instance.isDead())
        {
            StartCoroutine(FunnyEffect());
            return;
        } 

        if (!isStart && PlayerManager.Instance.Stage == PlayerManager.PlayerStage.Fight)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.down * 0.01f, ForceMode2D.Impulse);

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

    public bool PlayerIsInMeleeAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * meleeAttackOffset.x;
        pos += transform.up * meleeAttackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, meleeAttackRange, attackMask);
        if (colInfo != null)
            return true;
        return false;
    }

    public bool PlayerIsInRangedAttack()
    {
        if(hasRangedAttack)
        {
            Vector3 pos = transform.position;
            pos += transform.right * rangedAttackOffset.x;
            pos += transform.up * rangedAttackOffset.y;

            Collider2D colInfo = Physics2D.OverlapBox(pos, new Vector3(rangedAttackWidth, rangedAttackHeight), 0f, attackMask);
            if (colInfo != null)
                return true;
        }

        return false;
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * meleeAttackOffset.x;
        pos += transform.up * meleeAttackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, meleeAttackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK);
        }
    }

    public void RangedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * rangedAttackOffset.x;
        pos += transform.up * rangedAttackOffset.y;

        Collider2D colInfo = Physics2D.OverlapBox(pos, new Vector3(rangedAttackWidth, rangedAttackHeight), 0f, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK);
        }
    }

    public void AngryAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * meleeAttackOffset.x;
        pos += transform.up * meleeAttackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, meleeAttackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK + 10);
        }
    }

    public void RangedAngryAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * rangedAttackOffset.x;
        pos += transform.up * rangedAttackOffset.y;

        Collider2D colInfo = Physics2D.OverlapBox(pos, new Vector3(rangedAttackWidth, rangedAttackHeight), 0f, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(ATK + 10);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 meleePos = transform.position;
        meleePos += transform.right * meleeAttackOffset.x;
        meleePos += transform.up * meleeAttackOffset.y;

        Gizmos.DrawWireSphere(meleePos, meleeAttackRange);

        if(hasRangedAttack)
        {
            Gizmos.color = Color.red;
            Vector3 rangedPos = transform.position;
            rangedPos += transform.right * rangedAttackOffset.x;
            rangedPos += transform.up * rangedAttackOffset.y;
            Gizmos.DrawWireCube(rangedPos, new Vector3(rangedAttackWidth, rangedAttackHeight, 1));
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        float lossHealth = (1 - DEF / 10.0f) * damage;
        health = Mathf.Clamp(health - lossHealth, 0, HP);

        if (health < HP * 0.3)
        {
            anim.SetBool("isAngry", true);
        } 


        if (health <= 0)
        {
            Die();
        } else
        {
            //anim.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        isInvulnerable = true;
        StartCoroutine(DeathEffect());
    }

    private IEnumerator FunnyEffect()
    {

        anim.SetBool("isWin", true); 
        yield return new WaitForSeconds(2f);

        // Đóng băng
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isStart = false;
        grounded = false;
        isInvulnerable = false;

        anim.SetBool("isWin", false);
        anim.SetBool("isStart", false);
        anim.SetBool("isAngry", false);
        anim.SetBool("grounded", false);
        health = HP;
        transform.position = originPosition;
    }

    private IEnumerator DeathEffect()
    {
        yield return new WaitForSeconds(deathEffectTimer);
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

    //private void CheckGrounded()
    //{
    //    float raycastLength = 0.1f; // Độ dài của Raycast từ vị trí boss xuống mặt đất
    //    //Vector2 raycastOrigin = transform.position;
    //    //raycastOrigin.y -= 0.5f; // Độ cao của điểm bắt đầu Raycast từ trung tâm của boss

    //    // Kiểm tra xem có bề mặt dưới boss không
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, LayerMask.GetMask("Ground"));

    //    if (hit.collider != null)
    //    {
    //        if (!grounded)
    //            anim.SetBool("grounded", true);

    //        grounded = true;
    //        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    //    }
    //    //else
    //    //{
    //    //    grounded = false;
    //    //    anim.SetBool("grounded", false);
    //    //    rb.constraints = RigidbodyConstraints2D.None; // Mở khóa Y khi không ở trên mặt đất
    //    //}
    //}
}
