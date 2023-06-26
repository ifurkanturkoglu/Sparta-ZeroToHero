using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    int damage = 100;
    [SerializeField] AudioClip _audioClip;
    

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Equals("Enemy")){
            AudioController.Instance.audioSource.PlayOneShot(_audioClip);
            EffectController.Instance.EnemyDamageBlood(other.gameObject.transform);
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
