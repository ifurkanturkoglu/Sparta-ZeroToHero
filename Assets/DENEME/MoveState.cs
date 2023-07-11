using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    Vector3 newPos;
    Vector3 direction;
    bool isRun;
    public float animSpeed;

    public override void EnterState(PlayerControllerD player)
    {
        
    }


    public override void UpdateState(PlayerControllerD player)
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            player.SwitchState(player.attackState);
        }
        if (player.horizontal == 0 && player.vertical == 0)
        {
            ExitState(player);
            player.SwitchState(player.idleState);
        }
        newPos = new Vector3(player.horizontal, 0, player.vertical);
        if(Mathf.Abs(player.horizontal) > 0.001f || Mathf.Abs(player.vertical) > 0.001f){
            direction = new Vector3(player.horizontal, 0, player.vertical).normalized;
            //Dönme düzeltilecek
            player.transform.rotation = Quaternion.LookRotation(direction);
        }
            
        player.transform.localPosition += newPos * player.speed;

        isRun = Input.GetKey(KeyCode.LeftShift);

        player.speed = isRun ? 7.5f : 5;
        player.animator.SetFloat("speed", animSpeed);

        if(Input.GetKeyDown(KeyCode.Space)){
            player.animator.SetTrigger("Roll");
        }
        if (player.speed <= 5 && animSpeed <= 5){
            animSpeed += Time.deltaTime * player.speed * 2;
        }
        if (isRun && animSpeed <= 7.5f)
        {
            animSpeed += Time.deltaTime * player.speed * 2;
        }
        else if (!isRun && animSpeed > 5)
        {
            animSpeed -= Time.deltaTime * player.speed * 3;
        }
         if ((Input.inputString.Equals("1") || Input.inputString.Equals("2") || Input.inputString.Equals("3")))
        {
            player.SwitchState(player.skillState);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.SwitchState(player.interactionState);
        }
    }
    public override void ExitState(PlayerControllerD player)
    {
        //Interaction 
        //player.SwitchState()
    }

    public override void OnTriggerEnter(PlayerControllerD player)
    {
       
    }

    public override void OnTriggerStay(PlayerControllerD player,Collider other)
    {
        
    }
}
