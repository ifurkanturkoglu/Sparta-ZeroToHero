using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Interactable : MonoBehaviour
{
    public abstract AnimatorOverrideController animatorOverrideController{get;set;}
    public abstract Transform targetPosition{get;set;}
    public abstract void Interaction();
}
