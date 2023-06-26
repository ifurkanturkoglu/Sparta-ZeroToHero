using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionState : PlayerState
{
    string inputName;
    public override void EnterState(PlayerControllerD player)
    {
        player.interactable.Interaction();
    }
    public override void UpdateState(PlayerControllerD player)
    {
        
    }
    public override void ExitState(PlayerControllerD player)
    {
        
    }
    public override void OnTriggerEnter(PlayerControllerD player)
    {
        
    }

    public override void OnTriggerStay(PlayerControllerD player,Collider other)
    {
        player.inInteractableArea = true;
        if (other.tag.Equals("Interactable"))
        {
            player.interactable = other.gameObject.GetComponent<Interactable>();
            //Yazı kısmı düzelecek information
            if(GameManager.Instance.waveComplete && !Elevator.Instance.elevatorIsRun){
                UIManager.Instance.InformationTextUpdate(UIManager.Instance.interactionInfoText,Color.white);
            }
            else{
                UIManager.Instance.InformationTextUpdate("",Color.white);
            }
        }
    }
}
