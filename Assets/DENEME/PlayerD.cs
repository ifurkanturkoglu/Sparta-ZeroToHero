using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerD : MonoBehaviour
{
    protected static float maxHealth = 100;
    protected static float health;
    protected static float  maxStamina = 100;
    protected static float stamina;

    Coroutine damageCoroutine;
    [SerializeField] AudioClip damageSound, deathSound; //bunlar taşınacak
    void Awake()
    {
        health = 100;
        stamina = 100;    
    }

    void Start()
    {
        
    }

    public float GetHealth()
    {
        //print("health::::"+health);
        return health;
    }
    public void SetHealth(float value){
        health = value;
    }
    public void IncreaseHealth(float healthPercent){
        health += healthPercent;
    }
    public float GetStamina()
    {
        //print("stamina:::"+stamina);
        return stamina;
    }


    public void StaminaChange(float staminaPercent,bool increaseType){
        float increaceSign = increaseType ? 1 :-1;
        stamina += staminaPercent*increaceSign;
    }


    public void PlayerTakeDamage(Enemy? enemy, float? damagePercent)
    {
        float damage = 0;
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        if (enemy != null)
        {
            damage = PlayerController.Instance.isShieldActive ? enemy.damage - 2 : enemy.damage;
            damageCoroutine = StartCoroutine(DamageEffectTime());
        }
        else if (damagePercent != null)
        {
            damage = (float)(PlayerController.Instance.isShieldActive ? damagePercent - 15 : damagePercent);
            damageCoroutine = StartCoroutine(DamageEffectTime());
        }
        health -= damage;
        CameraController.Instance.ScreenShake(0.1f);
        StartCoroutine(UIManager.Instance.UpdateStaminaOrHPBar(UIManager.Instance.hpBar,damage, false, null,15));
        AudioController.Instance.audioSource.PlayOneShot(damageSound);
        if (health <= 0)
        {
            AudioController.Instance.audioSource.PlayOneShot(deathSound);
            PlayerController.Instance.animator.SetBool("dead", true);
            UIManager.Instance.FinishUIOpen();
        }
    }
    IEnumerator DamageEffectTime()
    {
        PlayerController.Instance.animator.SetTrigger("isDamaged");
        PlayerController.Instance.vignette.smoothness.value = .4f;
        while (PlayerController.Instance.vignette.smoothness.value > 0f)
        {
            PlayerController.Instance.vignette.smoothness.value -= Time.deltaTime;
            yield return null;
        }
    }
}
