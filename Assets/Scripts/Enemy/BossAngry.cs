using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAngry : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Boss boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        rb = animator.GetComponent<Rigidbody2D>();

        boss.isInvulnerable = true;
        rb.transform.position += new Vector3(0, 0.1f, 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.isInvulnerable = false;
        rb.transform.position -= new Vector3(0, 0.1f, 0);
    }
}
