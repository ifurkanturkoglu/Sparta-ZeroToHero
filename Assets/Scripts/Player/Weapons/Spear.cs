using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    int damage = 100;


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Equals("Enemy")){
            EffectController.Instance.EnemyDamageEffect(other.gameObject.transform);
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
