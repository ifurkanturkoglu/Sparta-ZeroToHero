using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttack")]
public class AttackTypeEnemy : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}

