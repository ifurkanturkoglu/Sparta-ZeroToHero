using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : PlayerState
{
    
    public override void EnterState(PlayerControllerD player)
    {
        SkillController skillController = player.GetComponent<SkillController>();
        skillController.UseSkill(player.animator,int.Parse(Input.inputString),player.GetStamina());
    }

    public override void ExitState(PlayerControllerD player)
    {
        
    }

    public override void OnTriggerEnter(PlayerControllerD player)
    {
        
    }

    public override void OnTriggerStay(PlayerControllerD player, Collider gameObject)
    {
        
    }

    public override void UpdateState(PlayerControllerD player)
    {
        
    }
}