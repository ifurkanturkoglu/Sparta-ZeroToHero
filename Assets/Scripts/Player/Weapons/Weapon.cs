using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract List<AttackType> attackTypes{get;set;}
    public abstract float damage{get;set;}
    public abstract float comboResetTime{get;set;}
    public abstract Collider weaponCollider{get;set;}

    public enum WeaponType{
        Sword,Axe,Spear
    }

    
}
