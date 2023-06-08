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
        transform.Rotate(moveDirection);
        //karakterin hareketi düzeltilecek. yönünü düzgün yapılması lazım.
        while(Vector3.Distance(transform.position,target.position) > 1f){
            PlayerController.Instance.animator.SetBool("isInteractionAnimation",true);
            transform.position += moveDirection *Time.deltaTime;
            yield return null;
        }
        PlayerController.Instance.animator.SetBool("isInteractionAnimation",false);
        PlayerController.Instance.animator.runtimeAnimatorController = interactable.animatorOverrideController;
        PlayerController.Instance.animator.Play("Interaction",2);
        yield return new WaitForSeconds(1);
    }
    #endregion
}
