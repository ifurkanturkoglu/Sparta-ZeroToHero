using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using UnityEditor;
using System;

public class PlayerController : Player
{
    [SerializeField] Volume volume;
    Vignette vignette;
    public static PlayerController Instance;
    public Animator animator;
    Coroutine damageCoroutine;

    [Header("Movement")]

    [SerializeField] float speed = 0;

    Rigidbody rb;
    Vector3 newPos;
    [SerializeField] bool isRun, isRoll;
    float horizontal, vertical, animSpeed, dashTime;


    [Header("Combat")]
    bool isChangingAttack;

    [SerializeField] List<AttackType> attackTypesPlayer;
    // [SerializeField] AnimatorOverrideController rightClickAttakAnim;
    [SerializeField] int attackTypeCount;
    public Weapon equipmentWeapon;
    Weapon.WeaponType playerEquiuppedWeaponType;
    public bool isAttack, canAttack;
    [SerializeField] float comboTimerForClickAttack;
    Collider equimentWeaponCollider;

    [Header("Skills")]
    public List<SkillType> skillTypes;
    [SerializeField] GameObject spear, sword;
    [SerializeField] Transform spearSpawnPoint;
    [SerializeField] bool skillActive,isShieldActive;
    [SerializeField] Material shieldMaterial;
    float forceRadius = 10f;
    float[] skillsCooldown = { 5, 10, 15 };
    Rigidbody spearRb;



    [Header("Interaction")]
    [SerializeField] Interactable interactable;
    bool inInteractableArea;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        UIManager.Instance.GameStartUpdateUI(health, stamina);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spearRb = spear.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        volume.profile.TryGet(out vignette);

        Color color = new Color(0,0,0,1);
        shieldMaterial.SetColor("_Color",color);

        foreach (var skill in skillTypes.Select((value, index) => (value, index)))
        {
            skill.value.useSkill = true;
            skill.value.cooldown = skillsCooldown[skill.index];
        }
        ChangeWeapon();
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;


        #region Move
        if (health > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
            {
                animator.StopPlayback();
                Dash();
            }
            if ((horizontal != 0 || vertical != 0) && !isAttack)
            {
                Move();

                if (speed <= 5 && animSpeed <= 5)
                {
                    animSpeed += Time.deltaTime * speed * 2;
                }
                if (isRun && animSpeed <= 7.5f)
                {
                    animSpeed += Time.deltaTime * speed * 2;
                }
                else if (!isRun && animSpeed >= 5)
                {
                    animSpeed -= Time.deltaTime * speed * 2;
                }
            }
            else if (animSpeed >= 0)
            {
                animSpeed -= Time.deltaTime * speed * 8;
            }

            isRun = Input.GetKey(KeyCode.LeftShift) && !isAttack;
            speed = isRun ? 7.5f : 5;
            animator.SetFloat("speed", animSpeed);


            #endregion
            #region Combat
            //Weapon ile combata bakılacak.
            isAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ? true : false;
            if ((Input.GetMouseButtonDown(0)) && !isAttack && !isRun && !isRoll)
            {
                Attack(equipmentWeapon);
            }
            if (comboTimerForClickAttack < equipmentWeapon.comboResetTime)
            {
                comboTimerForClickAttack += Time.deltaTime;
            }
            else
            {
                ResetComboCount();
            }


            #endregion

            #region Interaction

            if (inInteractableArea && Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interaction();
            }

            #endregion
            #region SkillAndPotion
            if ((Input.inputString.Equals("1") || Input.inputString.Equals("2") || Input.inputString.Equals("3")) && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1)
            {
                UseSkill(int.Parse(Input.inputString));
            }
            if (Input.GetKeyDown(KeyCode.F) && !isRun && !isRoll)
            {
                Pots.Instance.DrinkPot(Pots.Instance.equipmentPotType);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeDrinkPotionType();
            }
            #endregion
        }
    }

    #region MoveMethod
    void Move()
    {
        newPos = new Vector3(horizontal, 0, vertical);

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        //rb.velocity = new Vector3(horizontal*500,Physics.gravity.y,vertical*500);
        transform.localPosition += newPos * speed;

    }
    void Dash()
    {
        animator.SetTrigger("Roll");
    }
    void IsRollChange()
    {
        isRoll = !isRoll;
    }
    IEnumerator DashTimer()
    {
        while (dashTime < .25f)
        {
            dashTime += Time.deltaTime;
            transform.Translate(new Vector3(0, 0, 0.25f), Space.Self);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dashTime = 0;
    }
    #endregion


    #region AttackMethod

    void Attack(Weapon crWeapon)
    {
        if (!isChangingAttack)
        {
            StartCoroutine(ChangeAttackType(crWeapon));
        }
    }

    IEnumerator ChangeAttackType(Weapon crWeapon)
    {
        if (isChangingAttack) yield break;

        isChangingAttack = true;
        equipmentWeapon.GetComponent<Sword>().swordAttackEffect.enabled = true;
        if (attackTypeCount == attackTypesPlayer.Count)
        {
            attackTypeCount = 0;
        }
        comboTimerForClickAttack = 0;
        animator.runtimeAnimatorController = crWeapon.attackTypes[attackTypeCount].animatorOV;
        animator.SetTrigger("Attack");
        attackTypeCount++;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null;
        }
        equipmentWeapon.GetComponent<Sword>().swordAttackEffect.enabled = false;
        isChangingAttack = false;
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
    // public void HitAnimationSpeedIncrease(float speed)
    // {
    //     animator.SetFloat("attackAnimationSpeed", speed);
    // }
    // public void HitAnimationSpeedDecrease()
    // {
    //     animator.SetFloat("attackAnimationSpeed", 1);
    // }
    public void OneShotPlaySound(string clipName)
    {
        string path = "Assets/Resources/Sounds/Player/" + clipName + ".wav";
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    #endregion

    #region InteractionMethods
    void PlayerTakeDamage(Enemy enemy)
    {
        float damage = isShieldActive ? enemy.damage - 2 : enemy.damage;
        health -= damage;
        CameraController.Instance.ScreenShake(0.1f);
        StartCoroutine(UIManager.Instance.UpdateHpBar(enemy.damage, health, false));
        if (health <= 0)
        {
            animator.SetBool("dead", true);
        }
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        if (enemy.isAttack)
            damageCoroutine = StartCoroutine(DamageEffectTime());

    }
    IEnumerator DamageEffectTime()
    {
        animator.SetTrigger("isDamaged");
        vignette.smoothness.value = .4f;
        while (vignette.smoothness.value > 0f)
        {
            vignette.smoothness.value -= Time.deltaTime;
            yield return null;
        }
    }


    #endregion

    #region SkillMethods


    void UseSkill(int skillType)
    {
        SkillType skill;
        skill = skillTypes[skillType - 1];
        animator.runtimeAnimatorController = skillTypes[(int)skill.skillEffect - 1].animatorOverrideController;
        if (skill.useSkill)
        {
            switch (skill.skillEffect)
            {
                case SkillType.SkillEffect.Damage:
                    Spear(skillTypes[skillType - 1].skillEffectScale);
                    StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                    break;
                case SkillType.SkillEffect.Force:
                    StartCoroutine(CameraController.Instance.skillAnimaton(skillType));
                    
                    StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                    break;
                case SkillType.SkillEffect.Shield:
                    Shield(skillTypes[skillType - 1].skillEffectScale);
                    StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                    break;
            }
            animator.Play("Skill", 1);
        }
    }
    delegate void ChangeCooldown(float time, bool isSkillUse);
    IEnumerator SkillCooldownCalculate(ChangeCooldown act, SkillType skill)
    {
        float firstValue = skill.cooldown;
        skill.useSkill = false;
        while (skill.cooldown >= 0)
        {
            skill.cooldown -= Time.deltaTime;
            yield return null;
            act(skill.cooldown, skill.useSkill);
        }
        skill.useSkill = true;
        act(firstValue, skill.useSkill);
    }
    void Spear(float damageScale)
    {
        spear.transform.position = spearSpawnPoint.position;
        sword.SetActive(false);
        spear.SetActive(true);
        spear.transform.rotation = Quaternion.LookRotation(transform.forward);
        spearRb.velocity = transform.forward*50;
        StartCoroutine(nameof(SpearDisable));
    }
    IEnumerator SpearDisable()
    {
        yield return new WaitForSeconds(.5f);
        sword.SetActive(true);
        yield return new WaitForSeconds(3);
        spear.SetActive(false);
        spearRb.velocity = Vector3.zero;
    }
    void Shield(float shieldScale)
    {
        Color shieldColor = shieldMaterial.color;
        shield +=50;
        Color color = new Color(0,5,5,1);
        shieldMaterial.SetColor("_Color",color);
        StartCoroutine(ShieldTimer());
    }
    IEnumerator ShieldTimer(){
        isShieldActive = true;
        yield return new WaitForSeconds(15);
        Color color = new Color(0,0,0,1);
        shieldMaterial.SetColor("_Color",color);
        isShieldActive = false;
    }
    public void Force(float forceScale)
    {
        Collider[] enemiesInArea = Physics.OverlapSphere(transform.position, forceRadius);
        foreach (Collider item in enemiesInArea)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            print(item.name);
            //burada bir layer sınırı koyulacak sadece düşmanları içermesi gerekli
            if(rb != null)
                rb.GetComponent<Rigidbody>().AddExplosionForce(100,transform.position,forceRadius,3f);
        }
        print("force");
    }


    void ChangeDrinkPotionType()
    {
        print((int)Pots.Instance.equipmentPotType);
        print(Enum.GetValues(typeof(Pots.PotsType)).Length);
        if ((int)Pots.Instance.equipmentPotType >= Enum.GetValues(typeof(Pots.PotsType)).Length - 1)
        {
            Pots.Instance.equipmentPotType = Pots.PotsType.Health;
        }
        else
        {
            Pots.Instance.equipmentPotType++;
        }
    }

    #endregion




    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("EnemyWeapon"))
        {
            Enemy attackEnemy = other.gameObject.GetComponentInParent<Enemy>();
            if (attackEnemy.isAttack)
                PlayerTakeDamage(attackEnemy);
        }
    }
    void OnTriggerStay(Collider other)
    {
        inInteractableArea = true;
        if (other.tag.Equals("Interactable"))
        {
            interactable = other.gameObject.GetComponent<Interactable>();
        }
    }
    void OnTriggerExit(Collider other)
    {
        inInteractableArea = false;
        interactable = null;
    }

    //-----------------------------------------
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