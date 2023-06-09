using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] List<AttackType> swordAttackTypes;

    public override List<AttackType> attackTypes { get {return swordAttackTypes;} set {value = swordAttackTypes;}}
    public override float damage { get; set; }
    public override float comboResetTime { get ; set ; }
    public override Collider weaponCollider { get ; set ; }

    public WeaponType weaponType = WeaponType.Axe;
    public override WeaponEffectType weaponEffectType { get ; set ; }

    void Awake()
    {
        comboResetTime = 1.5f;
        damage = 10;
        weaponCollider = GetComponent<Collider>();
        weaponEffectType = WeaponEffectType.Default;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public override void ChangeWeaponEffect(WeaponEffectType weaponEffect,Color color)
    {
        weaponEffectType = weaponEffect;
    }
}
