using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pots : Player
{
    public static Pots Instance;
    public int healthPotionCount;
    public int staminaPotionCount;
    public PotsType equipmentPotType = PotsType.Health;
    [SerializeField] AnimatorOverrideController potionDrink;


    void Awake()
    {
        Instance = this;
        healthPotionCount = 0;
        staminaPotionCount = 0;
    }


    public void DrinkPot(PotsType potType)
    {
        float increasePercent = 0;
        switch (potType)
        {
            case PotsType.Health:
                if (healthPotionCount > 0)
                {
                    healthPotionCount--;
                    increasePercent = health + 20 >= maxHealth ? maxHealth - health : 20;
                    health += increasePercent;
                    StartCoroutine(UIManager.Instance.UpdateHpBar(increasePercent, health, true));
                    UIManager.Instance.healthPotCountText.text = healthPotionCount.ToString();
                    PlayerController.Instance.animator.runtimeAnimatorController = potionDrink;
                    PlayerController.Instance.animator.Play("Interaction", 2);
                }
                break;
            case PotsType.Stamina:
                if (staminaPotionCount > 0)
                {
                    staminaPotionCount--;
                    increasePercent = stamina + 20 >= maxStamina ? maxStamina - stamina : 20;
                    stamina += increasePercent;
                     UIManager.Instance.staminaPotCountText.text = staminaPotionCount.ToString();
                    StartCoroutine(UIManager.Instance.UpdateStaminaBar(increasePercent, stamina, true));
                    PlayerController.Instance.animator.runtimeAnimatorController = potionDrink;
                    PlayerController.Instance.animator.Play("Interaction", 2);
                }
                break;
        }
    }
    public enum PotsType
    {
        Health, Stamina
    }
}
