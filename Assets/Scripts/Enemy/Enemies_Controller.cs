using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Controller : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float moveDistance = 5f; // Khoảng cách di chuyển
    private float scaleDirection = 0.25f;
    private bool isMovingRight = true;
    private float initialPositionX;


    private CapsuleCollider2D capsuleCollider;
    private GameObject mainPlayer;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private LayerMask groundLayer;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialPositionX = transform.position.x;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        mainPlayer = GetComponent<GameObject>();
    }
    private void Start()
    {
        if (isGrounded())
        {
            animator.SetBool("isRunning", true);
        }
        else if (!isGrounded())
        {
            animator.SetBool("isRunning", false);
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D groundLayerRaycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return groundLayerRaycastHit.collider != null;
    }
    private void Update()
    {

        Movement();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //mainPlayer = players[0];

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < 1.0f)
            {
                //mainPlayer = player;
                MoveTowardsNearestPlayer();
                animator.SetTrigger("Attack");
                break;
            }
            else
            { 
                Movement();
            }
        }
    }
    void Movement()
    {
        if (isGrounded())
        {
            if (isMovingRight)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                if (transform.position.x >= initialPositionX + moveDistance)
                {
                    isMovingRight = false;
                    Flip();
                }
            }
            else
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                if (transform.position.x <= initialPositionX)
                {
                    isMovingRight = true;
                    Flip();
                }
            }
        }
    }

    void MoveTowardsNearestPlayer()
    {
        if (isGrounded())
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            // if (players.Length == 0)
            // {
            //     return;// Không có người chơi nào tồn tại
            // }

            Transform nearestPlayer = players[0].transform;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (GameObject potentialPlayer in players)
            {
                Vector3 directionToTarget = potentialPlayer.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    nearestPlayer = potentialPlayer.transform;
                }
            }

            Vector3 direction = (nearestPlayer.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Điều chỉnh hướng nhìn của kẻ địch để luôn nhìn về phía người chơi gần nhất
            if (direction.x <= 0 && isMovingRight)
            {

                
                // Người chơi ở bên trái, quay về phía trái
                //transform.localScale = new Vector3(-scaleDirection, scaleDirection, scaleDirection);
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                //isMovingRight = false;
                //Flip();
            }
            else 
            {
                // Người chơi ở bên phải, quay về phía phải
                //transform.localScale = new Vector3(scaleDirection, scaleDirection, scaleDirection);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                //Flip();
            }
        }
    }
    // private void AttackPlayer()
    // {

    // }

    //Đổi hướng
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
