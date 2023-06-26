using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract void EnterState(PlayerControllerD player);
    public abstract void UpdateState(PlayerControllerD player);    
    public abstract void ExitState(PlayerControllerD player);
    public abstract void OnTriggerEnter(PlayerControllerD player);
    public abstract void OnTriggerStay(PlayerControllerD player,Collider gameObject);
}
