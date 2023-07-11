using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    public override void EnterState(PlayerControllerD player)
    {
        Attack(player.equipmentWeapon, player);
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

    }

    public override void OnTriggerStay(PlayerControllerD player, Collider gameObject)
    {

    }

    void Attack(Weapon crWeapon, PlayerControllerD player)
    {   
        if(!player.isChangingAttack && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            player.StartCoroutine(ChangeAttackType(crWeapon, player));
    }
    

    IEnumerator ChangeAttackType(Weapon crWeapon, PlayerControllerD player)
    {
        if (player.isChangingAttack) yield break;
        
        player.isChangingAttack = true;
        crWeapon.GetComponent<Sword>().swordAttackEffect.enabled = true;

        if (player.attackTypeCount == player.attackTypesPlayer.Count)
        {
           player.attackTypeCount = 0;
        }
        player.comboTimerForClickAttack = 0;
        player.animator.runtimeAnimatorController = crWeapon.attackTypes[player.attackTypeCount].animatorOV;
        player.animator.SetTrigger("Attack");
        player.attackTypeCount++;
        
        while (player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            yield return null;
        }
        player.equipmentWeapon.GetComponent<Sword>().swordAttackEffect.enabled = false;
        player.isChangingAttack = false;
        ExitState(player);
    }
}