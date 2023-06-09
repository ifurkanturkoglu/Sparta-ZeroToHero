﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : Enemy
{
    public override EnemyAttackType enemyAttackType { get; set; }
    public override NavMeshAgent agent { get; set; }
    public override Animator animator { get; set; }
    public override GameObject target { get; set; }
    public override LayerMask playerLayer { get; set; }
    public override float health { get; set; }
    public override float damage { get; set; }
    public override float attackRange { get; set; }
    public override float speed { get; set; }
    public override float goldRate { get; set; }
    public override int score { get; set; }
    public override int gold { get; set; }
    public override bool isAttack { get; set; }
    public override bool isDamaged { get; set; }
    public override bool inSphereArea { get; set; }
    public override bool checkAttack { get; set; }
    public override bool isDead { get; set; }

    [SerializeField] List<AttackTypeEnemy> attackTypesEnemy = new();
    AttackTypeEnemy attackType;
    [SerializeField] bool isAttacking;
    [SerializeField] int damageCount = 0;
    int currentAttackIndex = 1;
    [SerializeField] bool isRaged;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");
        speed = agent.speed;
        attackRange = 2;
        health = 225;
        gold = 40;
        goldRate = 20;
        score = 200;
    }
    private void Update()
    {
        if (!isDamaged && !isDead)
        {
            checkAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            Movement();
            Raged();

            if (checkAttack && !isAttacking)
            {
                Attack();
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("rage"))
            {
                agent.isStopped = true;
            }
        }
        attackType = attackTypesEnemy[currentAttackIndex];
        damage = attackType.damage;
        Death();
        
    }
    public override void Movement()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            animator.SetFloat("speed", agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
        }
    }
    public override void Attack()
    {
        StartCoroutine(AttackChange());
    }
    public IEnumerator AttackChange()
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        currentAttackIndex = damageCount < 2 ? 1 : 0;
        //currentAttackIndex = health < 150 ? 1 : 0;
        animator.runtimeAnimatorController = attackTypesEnemy[currentAttackIndex].animatorOV;
        animator.SetTrigger("Attack");
        if (checkAttack)
            StartCoroutine(InAreaAttack());
        yield return null;
        isAttacking = false;
        animator.ResetTrigger("Attack");
    }
    public override IEnumerator InAreaAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
            transform.LookAt(target.transform);
        }
    }
    public override void TakeDamage(float damage)
    {
        health -= damage;
        isAttack = false;
        animator.SetTrigger("damage");
    }
    public override void Death()
    {
        if (health <= 0 && !isDead)
        {
            agent.isStopped = true;
            isDead = true;
            animator.SetTrigger("isDead");
            animator.SetBool("isDeadBool", isDead);
            GameManager.Instance.UpdateScore(score);
            CreateGold(transform, gold);
            GameManager.Instance.dieEnemyCount++;
            if (GameManager.Instance.waveComplete)
            {
                UIManager.Instance.InformationTextUpdate(UIManager.Instance.waveInfoText, Color.green);
            }
            Destroy(gameObject, 3);
        }
    }
    private void Raged()
    {
        if (damageCount>=2 && !isRaged)
        {
            isRaged = true;
            animator.SetTrigger("rage");
        }
    }
    public void CheckAttackStatus() { isAttack = !isAttack; }
    public void CheckDamagedStatus(string boolType)
    {
        if (boolType == "true")
        {
            isDamaged = true;
            damageCount++;
        }
        else
        {
            isDamaged = false;
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Weapon") && PlayerController.Instance.canAttack && !isDead)
        {
            TakeDamage(PlayerController.Instance.equipmentWeapon.damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}