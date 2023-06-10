using System.Collections;
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
    private int currentAttackIndex = 0;
    [SerializeField] bool isAttacking = false;
    [SerializeField] int damageCount = 0;

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
    }
    private void Update()
    {
        if (!isDamaged && !isDead)
        {
            checkAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            Movement();
            if (checkAttack && !isAttacking)
            {
                Attack();
            }
        }
        attackType = attackTypesEnemy[currentAttackIndex];
        damage = attackType.damage;
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
        if (!isAttacking)
        {
            StartCoroutine(AttackChange());
        }

    }

    public IEnumerator AttackChange()
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        currentAttackIndex = damageCount < 5 ? 1 : 0;
        animator.runtimeAnimatorController = attackTypesEnemy[currentAttackIndex].animatorOV;
        animator.SetTrigger("Attack");
        if (!inSphereArea)
            StartCoroutine(InAreaAttack());
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }
    public override IEnumerator InAreaAttack()
    {
        inSphereArea = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        transform.LookAt(target.transform);
        inSphereArea = false;
    }

    public override void TakeDamage()
    {
        health -= PlayerController.Instance.equipmentWeapon.damage;
        isAttack = false;
        animator.SetTrigger("damage");
        if (health <= 0)
        {
            isDead = true;
            animator.SetTrigger("isDead");
            animator.SetBool("isDeadBool", isDead);
            CreateGold(transform,gold);
            
            Destroy(gameObject, 3);
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
            TakeDamage();

        }
    }
}