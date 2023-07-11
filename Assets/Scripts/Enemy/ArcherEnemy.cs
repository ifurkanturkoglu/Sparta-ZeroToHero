using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemy : Enemy
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

    public bool isAttacking = false;
    public bool check;
    [SerializeField] GameObject arrowPrefab;
    List<GameObject> arrowPrefabs;
    [SerializeField] Transform arrowSpawnPoint;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");
        speed = agent.speed;
        attackRange = 10;
        damage = 7;
        health = 150;
        gold = 25;
        goldRate = 30;
        score = 125;

        arrowPrefabs = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(arrowPrefab);
            obj.transform.SetParent(transform, false);
            obj.SetActive(false);
            arrowPrefabs.Add(obj);
        }

    }
    private void Update()
    {
        checkAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        //Vector3 direction = target.transform.position - transform.position;
        //RaycastHit hit;
        //check = Physics.Raycast(transform.position, direction, out hit, attackRange, playerLayer);

        Movement();

        Attack();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.isStopped = true;
        }
        Death();
    }
    public override void Movement()
    {
        if (!checkAttack)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
        }

    }
    public override void Attack()
    {
        if (checkAttack)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(InAreaAttack());
        }
        else
        {
            animator.ResetTrigger("Attack");
        }
    }
    public void ArrowGenerate()
    {
        for (int i = 0; i < arrowPrefabs.Count; i++)
        {
            if (!arrowPrefabs[i].activeInHierarchy)
            {
                arrowPrefabs[i].transform.position = arrowSpawnPoint.transform.position /*+ transform.TransformVector(0, 1.7f, 0)*/;
                arrowPrefabs[i].transform.rotation = transform.rotation;
                arrowPrefabs[i].SetActive(true);
                break;
            }
        }
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
        animator.SetTrigger("Damage");
    }


    public override void Death()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death");
            //animator.SetBool("isDeadBool", isDead);
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
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Weapon") && PlayerController.Instance.canAttack && !isDead)
        {
            TakeDamage(PlayerController.Instance.equipmentWeapon.damage);
        }
    }
    public void CheckAttackStatus(int checkAttack)
    { isAttack = checkAttack == 1 ? true : false; }
    public void CheckDamagedStatus(string boolType)
    {
        if (boolType == "true")
        {
            isDamaged = true;
        }
        else
        {
            isDamaged = false;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
