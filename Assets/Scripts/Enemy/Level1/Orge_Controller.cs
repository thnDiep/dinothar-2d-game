using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orge_Controller : MonoBehaviour
{
   public float moveSpeed = 5f; // Tốc độ di chuyển
    public float moveDistance = 5f; // Khoảng cách di chuyển
    private float scaleDirection = 0.25f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isMovingRight = true;
    private float initialPositionX;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialPositionX = transform.position.x;

    }
    private void Start()
    {
        animator.SetBool("isRunning", true); // Kích hoạt animation running khi di chuyển
    }

    private void Update()
    {

        Movement();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < 1.25f)
            {
                
                //animator.SetBool("isRunning", false);
                MoveTowardsNearestPlayer();
                animator.SetTrigger("Attack");

            }
            else
            {
                Movement();
                //animator.SetBool("isRunning", true);
            }
        }
    }
    void Movement()
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

    void MoveTowardsNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            // Không có người chơi nào tồn tại
            return;
        }

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
        if (direction.x < 0)
        {
            // Người chơi ở bên trái, quay về phía trái
            transform.localScale = new Vector3(-scaleDirection, scaleDirection, scaleDirection);
        }
        else if (direction.x > 0)
        {
            // Người chơi ở bên phải, quay về phía phải
            transform.localScale = new Vector3(scaleDirection, scaleDirection, scaleDirection);
        }
    }

    private void Flip()//đổi hướng
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
