using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] List<AttackType> swordAttackTypes;

    public override List<AttackType> attackTypes { get {return swordAttackTypes;} set {value = swordAttackTypes;}}
    public override float damage { get; set; }
    public override float comboResetTime { get; set ; }
    public override Collider weaponCollider { get ; set; }

    public WeaponType weaponType = WeaponType.Sword;
    [SerializeField] GameObject prevEnemy;
    bool isPlayingSound;
    
    void Awake()
    {
        damage = 25;
        comboResetTime = 2.25f;
        weaponCollider = GetComponent<Collider>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Enemy") && prevEnemy != other.gameObject && PlayerController.Instance.isAttack && !isPlayingSound)
        {
            prevEnemy = other.gameObject;
            isPlayingSound = true;
            //AudioController.Instance.attackAudioClips();
            EffectController.Instance.EnemyDamageEffect(other.gameObject.transform);
            CameraController.Instance.ScreenShake(0.07f);
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject == prevEnemy)
        {
            prevEnemy = null;
            isPlayingSound = false;
        }
    }
    
}
