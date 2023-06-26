using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    public override void EnterState(PlayerControllerD player)
    {
        
    }

    public override void UpdateState(PlayerControllerD player)
    {
        
    }
    public override void ExitState(PlayerControllerD player)
    {
        player.SwitchState(player.idleState);
    }

    public override void OnTriggerEnter(PlayerControllerD player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(PlayerControllerD player, Collider gameObject)
    {
        throw new System.NotImplementedException();
    }
}
