using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttackState : EnemyBaseState
{
    private Animator animator;
    private float attackDuration;
    NavMeshAgent agent;
    GameObject target;

    public override void EnterState(EnemyStateManager state)
    {
        agent = state.GetComponent<NavMeshAgent>();

        animator = state.GetComponent<Animator>();
        attackDuration = 1f;
        animator.SetTrigger("Attack");
        target = GameObject.Find("Player");
    }

    public override void UpdateState(EnemyStateManager state)
    {
        animator.ResetTrigger("Attack");
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.isStopped = false;

            state.SwitchState(state.MovementState);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.isStopped = true;

        }
       
    }

    public override void ExitState(EnemyStateManager state)
    {


    }

    public override void OnCollisionEnter(EnemyStateManager state)
    {
        throw new System.NotImplementedException();
    }
}
