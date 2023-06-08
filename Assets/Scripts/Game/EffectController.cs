using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    [SerializeField] ParticleSystem enemyDamageEffect;
    void Start()
    {
        if(Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDamageEffect(Transform enemyPosition){
        enemyDamageEffect.transform.position = enemyPosition.position;
        enemyDamageEffect.Play();
    }
}
