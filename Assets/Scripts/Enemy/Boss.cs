using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D collider2d;

    [SerializeField] private Transform[] players;
    private bool isLookAtRight = true;

    // Attack
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask attackMask;

    // Health
    [SerializeField] private int health = 500;
    private int currentHealth;

    public bool isInvulnerable = false;

    public GameObject deathEffect;
    public void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();

        currentHealth = health;
    }

    public void LookAtPlayer()
    {
        Transform player1 = players[0].transform;
        Transform player2 = players[1].transform;
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
            colInfo.GetComponent<PlayerController>().TakeDamage(attackDamage);
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
            colInfo.GetComponent<PlayerController>().TakeDamage(attackDamage * 2);
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

        currentHealth -= damage;
        Debug.Log("Boss Health:" + currentHealth);
        if(currentHealth < health * 0.3)
        {
            anim.SetBool("isAngry", true);
            Debug.Log("Boss is angry");
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
    }
}
