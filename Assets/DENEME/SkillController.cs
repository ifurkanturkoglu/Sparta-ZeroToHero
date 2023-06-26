using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] List<SkillType> skillTypes;
    [SerializeField] GameObject spear, sword;
    [SerializeField] Material shieldMaterial;
    [SerializeField] Transform spearSpawnPoint;
    LayerMask enemyLayerMask;
    Rigidbody spearRb;
    float forceRadius;
    public bool isShieldActive;
    float[] skillsCooldown = { 5, 10, 15 };

    void Start()
    {
        forceRadius = 10f;
        foreach (var skill in skillTypes.Select((value, index) => (value, index)))
        {
            skill.value.useSkill = true;
            skill.value.cooldown = skillsCooldown[skill.index];
        }
        spearRb = spear.GetComponent<Rigidbody>();
        Color color = new Color(0, 0, 0, 1);
        shieldMaterial.SetColor("_Color", color);
        

        enemyLayerMask = LayerMask.GetMask("Enemy");
        
    }
    public void UseSkill(Animator animator,int skillType,float stamina)
    {
        SkillType skill;
        skill = skillTypes[skillType - 1];
        animator.runtimeAnimatorController = skillTypes[(int)skill.skillEffect - 1].animatorOverrideController;
        if (skill.useSkill)
        {
            StartCoroutine(SkillCooldownCalculate(((b, n) => { skill.cooldown = b; skill.useSkill = n; }), skill));
            switch (skill.skillEffect)
            {
                case SkillType.SkillEffect.Damage:
                    if (stamina >= skill.mana)
                    {
                        Spear(skillTypes[skillType - 1].skillEffectScale);
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill1,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
                case SkillType.SkillEffect.Force:
                    if (stamina >= skill.mana)
                    {
                        StartCoroutine(CameraController.Instance.skillAnimaton(skillType));
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill2,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
                case SkillType.SkillEffect.Shield:
                    if (stamina >= skill.mana)
                    {
                        Shield(skillTypes[skillType - 1].skillEffectScale);
                        StartCoroutine(UIManager.Instance.SkillIconUpdate(UIManager.Instance.skill3,skill.cooldown));
                        animator.Play("Skill", 1);
                    }
                    break;
            }
            FindAnyObjectByType<PlayerD>().StaminaChange(skill.mana,false);
            StartCoroutine(UIManager.Instance.UpdateStaminaOrHPBar(UIManager.Instance.staminaBar,skill.mana,false,null,20));
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
                rb.AddExplosionForce(4000, transform.position, forceRadius, 3f, ForceMode.Force);
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
}
