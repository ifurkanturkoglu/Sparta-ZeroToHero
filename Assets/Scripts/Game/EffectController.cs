using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    [SerializeField] ParticleSystem enemyDamageEffect;
    [SerializeField] GameObject bloodEffect;
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
    public IEnumerator EnemyDamageBlood(Transform enemyPosition){
        print("girdi");
        bloodEffect.transform.position = new Vector3(enemyPosition.position.x,1, enemyPosition.position.z);
        bloodEffect.transform.rotation = Quaternion.LookRotation(enemyPosition.transform.right);
        bloodEffect.SetActive(true);
        yield return new WaitForSeconds(.5f);
        bloodEffect.SetActive(false);
    }
}
