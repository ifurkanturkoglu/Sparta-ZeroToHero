using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public static Elevator Instance;
    [SerializeField] float elevatorMoveTime,elevatorSpeed;
    public bool elevatorLocation,elevatorIsRun;

    [SerializeField] AnimatorOverrideController pushButtonAnim;
    [SerializeField] Transform target;

    public override AnimatorOverrideController animatorOverrideController { get{return pushButtonAnim;} set{value = pushButtonAnim;} }
    public override Transform targetPosition { get{ return target; } set{ value = target; } }

    void Start()
    {
        if(Instance == null)
            Instance = this;
    }
    public override void Interaction()
    {
        if(!elevatorIsRun && GameManager.Instance.waveComplete){
            StartCoroutine(nameof(ElevatorMove));
        }

            
    }
    
    IEnumerator ElevatorMove(){
        yield return StartCoroutine(PlayerAnimationController.Instance.InteractionWithMoveAndAnimation(this,targetPosition));
        UIManager.Instance.InformationTextUpdate("",Color.white);
        elevatorLocation = !elevatorLocation;
        elevatorIsRun = true;
        int upOrDown = elevatorLocation ? -1 : 1;
        float timer = 0;
        while(timer <= elevatorMoveTime){
            timer += Time.deltaTime;
            transform.position += Vector3.up*upOrDown*elevatorSpeed*Time.deltaTime;
            yield return null;
        }
        PlayerController.Instance.animator.SetBool("isInteractionAnimation",false);
        elevatorIsRun = false;
    }
}
