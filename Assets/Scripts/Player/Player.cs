using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected  static float maxHealth = 100;
    protected float health = 100;
    protected static float maxStamina = 100;
    protected float stamina = 100;
    protected float shield = 0;

    
    

    public static void BuffPassives(PassiveStatus status){
        switch(status){
            case PassiveStatus.Damage:
                PlayerController.Instance.equipmentWeapon.damage +=5;
                break;
            case PassiveStatus.Health:
                maxHealth += 20;
                UIManager.Instance.hpBar.maxValue += 20;
                break;    
            case PassiveStatus.Stamina:
                maxStamina += 20;
                UIManager.Instance.staminaBar.maxValue += 20;
                break;
        }
        print(maxHealth+"--"+maxStamina+"---"+PlayerController.Instance.equipmentWeapon.damage);
    }
    public enum PassiveStatus{
        Health,Stamina,Damage
    }
}
