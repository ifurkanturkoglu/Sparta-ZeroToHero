using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pots : Player
{
    public static new Pots Instance;
    public int healthPotionCount;
    public int staminaPotionCount;
    public PotsType equipmentPotType = PotsType.Health;
    [SerializeField] AnimatorOverrideController potionDrink;
    [SerializeField] GameObject healthPotion,staminaPotion;
    [SerializeField] AudioClip potSound;
    public bool canPotDrink;


    void Awake()
    {
        Instance = this;
        healthPotionCount = 0;
        staminaPotionCount = 0;
    }
   
    void Update()
    {
        
    }

    public void DrinkPot(PotsType potType)
    {
        float increasePercent = 0;
        
        if (!canPotDrink)
        {
            StartCoroutine(PotsDrinkCheck());
            switch (potType)
            {
                case PotsType.Health:
                    if (healthPotionCount > 0 && health < maxHealth)
                    {
                        healthPotionCount--;
                        increasePercent = health + 20 >= maxHealth ? maxHealth - health : 20;
                        IncreaseHealth(increasePercent);
                        print(health);
                        StartCoroutine(UIManager.Instance.UpdateStaminaOrHPBar(UIManager.Instance.hpBar,increasePercent, true,healthPotion,15));
                        UIManager.Instance.healthPotCountText.text = healthPotionCount.ToString();
                        PlayerController.Instance.animator.runtimeAnimatorController = potionDrink;
                        PlayerController.Instance.animator.Play("Interaction", 2);
                        AudioController.Instance.audioSource.PlayOneShot(potSound);
                    }
                    break;
                case PotsType.Stamina:
                    if (staminaPotionCount > 0 && stamina < maxStamina)
                    {
                        staminaPotionCount--;
                        increasePercent = stamina + 20 >= maxStamina ? maxStamina - stamina : 20;
                        StaminaChange(increasePercent,true);
                        print(stamina);
                        UIManager.Instance.staminaPotCountText.text = staminaPotionCount.ToString();
                        StartCoroutine(UIManager.Instance.UpdateStaminaOrHPBar(UIManager.Instance.staminaBar,increasePercent, true,staminaPotion,15));
                        PlayerController.Instance.animator.runtimeAnimatorController = potionDrink;
                        PlayerController.Instance.animator.Play("Interaction", 2);
                        AudioController.Instance.audioSource.PlayOneShot(potSound);
                    }
                    break;
            }
        }

    }

    IEnumerator PotsDrinkCheck()
    {
        if(canPotDrink) yield break;
        canPotDrink = true;
        yield return new WaitForSeconds(2);
        canPotDrink = false;
    }
    public enum PotsType
    {
        Health, Stamina
    }
}
