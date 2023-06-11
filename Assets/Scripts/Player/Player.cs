using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    protected  static float maxHealth = 100;
    protected  float  health = 100;
    protected static float maxStamina = 100;
    protected static float stamina = 100;
    protected float shield = 0;

    
    
    void Update()
    {
        if(Instance == null){
            Instance=this;
        }
        if(stamina <=100){
            stamina += Time.deltaTime*0.2f;
        }
    }

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
    }
    public static float GetStamina(){
        return stamina;
    }

    public enum PassiveStatus{
        Health,Stamina,Damage
    }
}
