using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    float timer =0;
    public override void EnterState(PlayerControllerD player)
    {
        
    }

    public override void UpdateState(PlayerControllerD player)
    {
        if ((Input.inputString.Equals("1") || Input.inputString.Equals("2") || Input.inputString.Equals("3")))
        {
            player.SwitchState(player.skillState);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.SwitchState(player.interactionState);
        }
        if(player.horizontal != 0 || player.vertical != 0){
            ExitState(player);
        }
        if (player.moveState.animSpeed >= 0)
        {
            player.moveState.animSpeed -= Time.deltaTime *player.speed * 8;
            player.animator.SetFloat("speed",player.moveState.animSpeed);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            player.animator.SetTrigger("Roll");
        }
    }
    public override void ExitState(PlayerControllerD player)
    {
       player.SwitchState(player.moveState);
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
