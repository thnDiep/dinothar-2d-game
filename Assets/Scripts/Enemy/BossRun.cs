using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private bool isAngry = false;

    private Rigidbody2D rb;
    private Boss boss;
    private Transform player1, player2;
    private float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        player1 = PlayerManager.Instance.player1.transform;
        player2 = PlayerManager.Instance.player2.transform;

        speed = boss.SPEED;

        if (isAngry)
            speed += 0.5f;

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
