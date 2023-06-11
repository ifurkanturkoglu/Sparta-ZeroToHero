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
    [SerializeField] Transform elevatorMidPosition;

    bool deneme;
    Vector3 minPosition, maxPosition;
    [SerializeField] float speed = 0;


    [SerializeField] Collider elevatorCollider;

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
    public bool isAttack, canAttack, isInteractionAnimation;
    [SerializeField] float comboTimerForClickAttack;
    Collider equimentWeaponCollider;

    [Header("Skills")]
    public List<SkillType> skillTypes;
    [SerializeField] GameObject spear, sword;
    [SerializeField] Transform spearSpawnPoint;
    [SerializeField] bool skillActive, isShieldActive;
    [SerializeField] Material shieldMaterial;
    LayerMask enemyLayerMask;
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

    }

    void Start()
    {
        UIManager.Instance.GameStartUpdateUI(health, stamina);
        rb = GetComponent<Rigidbody>();
        spearRb = spear.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        volume.profile.TryGet(out vignette);

        Color color = new Color(0, 0, 0, 1);
        shieldMaterial.SetColor("_Color", color);

        enemyLayerMask = LayerMask.GetMask("Enemy");

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


        if (elevatorCollider.bounds.Contains(transform.position))
        {
            deneme = true;
        }
        else
        {
            deneme = false;
        }
        #region Move
        if (health > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
            {
                animator.StopPlayback();
                Dash();
            }
            if ((horizontal != 0 || vertical != 0) && !isAttack && !isInteractionAnimation)
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
            if ((Input.GetMouseButtonDown(0)) && !isAttack && !isRun && !isRoll&& !isInteractionAnimation)
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

            if (inInteractableArea && Input.GetKeyDown(KeyCode.E) && !isInteractionAnimation)
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
        transform.localPosition += newPos * speed;
        if (deneme && Elevator.Instance.elevatorIsRun)
        {
            //alan sınırlaması yapılacak.
            Vector3 minVec = new Vector3(255f, 0, -9f);
            Vector3 maxVec = new Vector3(270f, 0, 6f);

            newPos = new Vector3(Mathf.Clamp(transform.position.x, minVec.x, maxVec.x), transform.position.y, Mathf.Clamp(transform.position.z, minVec.z, maxVec.z));

            transform.localPosition = newPos;
        }


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
    // public void OneShotPlaySound(string clipName)
    // {
    //     string path = "Assets/Resources/Sounds/Player/" + clipName + ".wav";
    //     AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    //     GetComponent<AudioSource>().PlayOneShot(clip);
    // }

    #endregion

    #region InteractionMethods
    public void PlayerTakeDamage(Enemy? enemy, float? damagePercent)
    {
        float damage = 0;
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        if (enemy != null)
        {
            damage = isShieldActive ? enemy.damage - 2 : enemy.damage;
            damageCoroutine = StartCoroutine(DamageEffectTime());
        }
        else if (damagePercent != null)
        {
            damage = (float)(isShieldActive ? damagePercent - 15 : damagePercent);
            damageCoroutine = StartCoroutine(DamageEffectTime());
        }
        health -= damage;
        CameraController.Instance.ScreenShake(0.1f);
        StartCoroutine(UIManager.Instance.UpdateHpBar(damage, health, false));
        if (health <= 0)
        {
            animator.SetBool("dead", true);
        }

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
                    if (stamina >= skill.mana)
                    {
                        stamina -= skill.mana;
                        Spear(skillTypes[skillType - 1].skillEffectScale);
                        StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill1,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
                case SkillType.SkillEffect.Force:
                    if (stamina >= skill.mana)
                    {
                        stamina -= skill.mana;
                        StartCoroutine(CameraController.Instance.skillAnimaton(skillType));
                        StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill2,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
                case SkillType.SkillEffect.Shield:
                    if (stamina >= skill.mana)
                    {
                        stamina -= skill.mana;
                        Shield(skillTypes[skillType - 1].skillEffectScale);
                        StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill3,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
            }

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
        spearRb.velocity = transform.forward * 50;
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
        shield += 50;
        Color color = new Color(0, 8, 8, 1);
        shieldMaterial.SetColor("_Color", color);
        StartCoroutine(ShieldTimer());
    }
    IEnumerator ShieldTimer()
    {
        isShieldActive = true;
        yield return new WaitForSeconds(15);
        Color color = new Color(0, 0, 0, 1);
        shieldMaterial.SetColor("_Color", color);
        isShieldActive = false;
    }
    public void Force(float forceScale)
    {
        Collider[] enemiesInArea = Physics.OverlapSphere(transform.position, forceRadius, enemyLayerMask);
        foreach (Collider item in enemiesInArea)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(8000, transform.position, forceRadius, 3f, ForceMode.Force);
            }
        }
        StartCoroutine(ThenForce(enemiesInArea));
    }
    IEnumerator ThenForce(Collider[] enemyRb)
    {
        yield return new WaitForSeconds(2);

        foreach (var item in enemyRb)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

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
    }

    #endregion




    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("EnemyWeapon"))
        {
            Enemy attackEnemy = other.gameObject.GetComponentInParent<Enemy>();
            if (attackEnemy.isAttack)
                PlayerTakeDamage(attackEnemy, null);
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