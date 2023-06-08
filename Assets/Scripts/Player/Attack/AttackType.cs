using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Attack")]
public class AttackType : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
