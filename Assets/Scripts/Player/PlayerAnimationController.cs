using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : Player
{
    public static PlayerAnimationController Instance;
    
    void Start()
    {
        Instance = this;
    }

    
    void Update()
    {
    }

    

    #region InteractionAnimation
    public IEnumerator InteractionWithMoveAndAnimation(Interactable interactable,Transform target){
        Vector3 moveDirection = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        PlayerController.Instance.isInteractionAnimation = true;
        //karakterin hareketi düzeltilecek. yönünü düzgün yapılması lazım.
        PlayerController.Instance.animator.SetBool("isInteractionAnimation",true);
        while(Vector3.Distance(transform.position,target.position) > 1f){
           
            transform.position += moveDirection *Time.deltaTime*3f;
            yield return null;
        }
        PlayerController.Instance.animator.SetBool("isInteractionAnimation",false);
        PlayerController.Instance.animator.runtimeAnimatorController = interactable.animatorOverrideController;
        PlayerController.Instance.animator.Play("Interaction",2);
        yield return new WaitForSeconds(1);
        PlayerController.Instance.isInteractionAnimation = false;
    }
    #endregion
}
