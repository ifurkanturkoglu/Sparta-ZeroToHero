using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : Enemy
{
    public override float health { get; set; }
    public override float damage { get; set; }
    public override NavMeshAgent agent { get; set; }
    public override Animator animator { get; set; }
    public override GameObject target { get; set; }
    public override float attackRange { get; set; }
    public override LayerMask playerLayer { get; set; }
    public override bool isAttack { get; set; }
    public override bool isDamaged { get; set; }
    public override bool inSphereArea { get; set; }
    public override bool checkAttack { get; set; }
    public override bool isDead { get; set; }
    public override EnemyAttackType enemyAttackType { get; set; }
    public override float speed { get; set; }
    public override int score { get; set; }
    public override int gold { get; set; }
    public override float goldRate { get; set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");
        speed = agent.speed;
        attackRange = 1;
        health = 100;
        damage = 5;
        gold = 10;
        goldRate = 25;
        score = 50;

    }
    private void Update()
    {
        if (!isDamaged && !isDead)
        {
            Movement();
            Attack();
        }
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
        checkAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (checkAttack)
        {
            animator.SetBool("IsAttack", true);
            if (!inSphereArea)
                StartCoroutine(InAreaAttack());
        }
        else if (!checkAttack)
        {
            animator.SetBool("IsAttack", false);
        }
    }

    public override IEnumerator InAreaAttack()
    {
        inSphereArea = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        transform.LookAt(target.transform);
        inSphereArea = false;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("damage");
        if (health <= 0)
        {
            isDead = true;
            animator.SetTrigger("isDead");
            animator.SetBool("isDeadBool", isDead);
            GameManager.Instance.UpdateScore(score);
            CreateGold(transform, gold);
            GameManager.Instance.dieEnemyCount++;
            if(GameManager.Instance.waveComplete){
                UIManager.Instance.InformationTextUpdate(UIManager.Instance.waveInfoText,Color.green);
            }
            Destroy(gameObject, 3);
        }
    }
    public void CheckAttackStatus() { isAttack = !isAttack; }
    public void CheckDamagedStatus(string boolType) { isDamaged = boolType == "true" ? true : false; }
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Weapon") && PlayerController.Instance.canAttack&& !isDead)
        {
            TakeDamage(PlayerController.Instance.equipmentWeapon.damage);
        }
    }
}
