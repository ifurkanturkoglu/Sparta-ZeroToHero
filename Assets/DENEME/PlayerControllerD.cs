using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerControllerD : PlayerD
{
    public TextMeshProUGUI currentStateText;
    public Animator animator;

    [Header("States")]
    public PlayerState currentState;
    public MoveState moveState = new MoveState();
    public AttackState attackState = new AttackState();
    public IdleState idleState = new IdleState();
    public InteractionState interactionState = new InteractionState();
    public SkillState skillState = new SkillState();

    [Header("Move")]
    public float horizontal, vertical;
    public float speed;

    [Header("Combat")]
    public List<AttackType> attackTypesPlayer;
    public Weapon equipmentWeapon;
    public float comboTimerForClickAttack;
    public bool isChangingAttack,canAttack;
    public int attackTypeCount = 0;
    Weapon.WeaponType playerEquiuppedWeaponType;
    Collider equimentWeaponCollider;

    [Header("Interaction")]
    public Interactable interactable;
    public bool inInteractableArea;

    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
        currentStateText.text = currentState.ToString();

        ChangeWeapon();
    }


    void Update()
    {
        //SKİLL SİSTEMİ INTERACTİON VE POT SİSTEMİNE BAK 
        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;
        //skill kullanma birden fazla statede bulunacak fakat bazı statelerde bulunmayacak bunu interfacelerle ya da her state'e tekrar tekrar yaz

        if (Input.GetMouseButtonDown(0))
        {
            SwitchState(attackState);
        }
        if (Input.GetKeyDown(KeyCode.F) && currentState != interactionState)
        {
            Pots.Instance.DrinkPot(Pots.Instance.equipmentPotType);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeDrinkPotionType();
        }

        if (comboTimerForClickAttack < equipmentWeapon.comboResetTime)
        {
            comboTimerForClickAttack += Time.deltaTime;
        }
        else
        {
            ResetComboCount();
            print("else");
        }

        currentState.UpdateState(this);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("EnemyWeapon"))
        {
            Enemy attackEnemy = other.gameObject.GetComponentInParent<Enemy>();
            if (attackEnemy.isAttack && health > 0)
                PlayerTakeDamage(attackEnemy, null);
        }
        currentState.OnTriggerEnter(this);
    }
    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(this, other);
    }

    void OnTriggerExit(Collider other)
    {
        inInteractableArea = false;
        interactable = null;
    }
    public void SwitchState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
        currentStateText.text = currentState.ToString();
    }


    void ChangeDrinkPotionType()
    {
        if ((int)Pots.Instance.equipmentPotType >= Enum.GetValues(typeof(Pots.PotsType)).Length - 1)
        {
            Pots.Instance.equipmentPotType = Pots.PotsType.Health;
        }
        else
        {
            Pots.Instance.equipmentPotType++;
        }
        //Pot değiştirme değişebilir.
        // foreach (var item in Enum.GetValues(typeof(Pots.PotsType)))
        // {
        //     if(item.Equals(Pots.Instance.equipmentPotType)){

        //     }
        // }
        switch (Pots.Instance.equipmentPotType)
        {
            case Pots.PotsType.Health:
                UIManager.Instance.healthPotCountText.color = Color.green;
                UIManager.Instance.staminaPotCountText.color = Color.white;
                break;
            case Pots.PotsType.Stamina:
                UIManager.Instance.healthPotCountText.color = Color.white;
                UIManager.Instance.staminaPotCountText.color = Color.green;
                break;
        }
    }


    void ResetComboCount()
    {
        attackTypeCount = 0;
        comboTimerForClickAttack = 0;
    }
    public void CheckAttack(string check)
    {
        canAttack = check.Equals("true") ? true : false;
    }

    void ChangeWeapon()
    {
        //buraya değiştirilen silahın ve değiştirilmiş olan silahın bilgileri gerekli ki setactive ile kapatıp açmamız gerekli.
        equipmentWeapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
        if (equipmentWeapon != null)
        {
            if (equipmentWeapon is Sword sword)
            {
                attackTypesPlayer = sword.attackTypes;
                playerEquiuppedWeaponType = sword.weaponType;
                equimentWeaponCollider = sword.weaponCollider;
            }
            else if (equipmentWeapon is Axe axe)
            {
                attackTypesPlayer = axe.attackTypes;
                playerEquiuppedWeaponType = axe.weaponType;
                equimentWeaponCollider = axe.weaponCollider;
            }
        }
    }

}
