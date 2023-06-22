using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementState : EnemyBaseState
{
    NavMeshAgent agent;
    Animator animator;
    GameObject target;
    bool checkAttack;
    float attackRange;
    LayerMask playerLayer;

    public override void EnterState(EnemyStateManager state)
    {
        agent = state.GetComponent<NavMeshAgent>();
        animator = state.GetComponent<Animator>();
        target = GameObject.Find("Player");

        attackRange = 2;
        playerLayer = LayerMask.GetMask("Player");
    }
    public override void UpdateState(EnemyStateManager state)
    {
        checkAttack = Physics.CheckSphere(state.transform.position, attackRange, playerLayer);

        agent.SetDestination(target.transform.position);

        animator.SetFloat("speed", agent.velocity.magnitude);
        Debug.Log("movement");
        if (checkAttack)
        {
            agent.isStopped = true;

            state.SwitchState(state.AttackState);
            Debug.Log("attacka geçiş");
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