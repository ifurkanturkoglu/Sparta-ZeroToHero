using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public abstract EnemyAttackType enemyAttackType { get; set; }
    public enum EnemyAttackType { Melee, Ranger }
    public abstract NavMeshAgent agent { get; set; }
    public abstract Animator animator { get; set; }
    public abstract GameObject target { get; set; }
    public abstract LayerMask playerLayer { get; set; }
    public abstract float health { get; set; }
    public abstract float damage { get; set; }
    public abstract float attackRange { get; set; }
    public abstract float speed { get; set; }
    public abstract float goldRate { get; set; }
    public abstract int score { get; set; }
    public abstract int gold { get; set; }
    public abstract bool isAttack { get; set; }
    public abstract bool isDamaged { get; set; }
    public abstract bool inSphereArea { get; set; }
    public abstract bool checkAttack { get; set; }
    public abstract bool isDead { get; set; }
    public abstract void Movement();
    public abstract void Attack();
    public abstract void Death();
    public abstract void TakeDamage(float damage);
    public abstract IEnumerator InAreaAttack();

    public void CreateGold(Transform createPos, int goldRate)
    {
        GameObject cloneGold = Instantiate(GameManager.Instance.goldPrefab, createPos.position + new Vector3(0, 1, 0), createPos.rotation);
        cloneGold.GetComponent<Gold>().gold = goldRate;
        cloneGold.transform.SetParent(GameManager.Instance.goldsParent);
    }
}
