using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float attackRange = 1.5f;

    private Rigidbody2D rb;
    private Boss boss;
    private Transform player1, player2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 2)
        {
            player1 = players[0].transform;
            player2 = players[1].transform;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        float distance1 = Vector2.Distance(player1.position, rb.position);
        float distance2 = Vector2.Distance(player2.position, rb.position);

        Vector2 targetPos;
        float distance;

        if (distance1 <= distance2)
        {
            targetPos = new Vector2(player1.position.x, rb.position.y);
            distance = distance1;
        } else
        {
            targetPos = new Vector2(player2.position.x, rb.position.y);
            distance = distance2;
        }

        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (distance <= attackRange)
            animator.SetTrigger("Attack");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
