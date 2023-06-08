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

    
    void Awake()
    {
        comboResetTime = 1.5f;
        damage = 10;
        weaponCollider = GetComponent<Collider>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
