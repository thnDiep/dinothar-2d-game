using UnityEngine;

public class MinionRun : StateMachineBehaviour
{
    [SerializeField] private float attackRange = 0.5f;

    private Rigidbody2D rb;
    private Minion minion;
    private Transform player1, player2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        minion = animator.GetComponent<Minion>();
        player1 = PlayerManager.Instance.player1.transform;
        player2 = PlayerManager.Instance.player2.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance1 = Vector2.Distance(player1.position, rb.position);
        float distance2 = Vector2.Distance(player2.position, rb.position);

        if (minion.IsPlayer1InRange() && minion.IsPlayer2InRange())
        {
            if (distance1 <= distance2)
            {
                minion.FollowPlayer1();
            }
            else
            {
                minion.FollowPlayer2();
            }
        }
        else if (minion.IsPlayer1InRange())
        {
            minion.FollowPlayer1();
        }
        else if (minion.IsPlayer2InRange())
        {
            minion.FollowPlayer2();
        }
        else
        {
            minion.Move();
        }

        if (distance1 <= attackRange || distance2 <= attackRange)
        {
            animator.SetTrigger("Attack");
            minion.isAttack = true;
        }
        else
        {
            minion.isAttack = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
