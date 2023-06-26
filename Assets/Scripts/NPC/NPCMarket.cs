using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCMarket : Interactable
{
    [SerializeField] AnimatorOverrideController playerNpcInteractionAnim;
    [SerializeField] Transform target;
    [SerializeField] GameObject MarketUI;
    public override AnimatorOverrideController animatorOverrideController { get { return playerNpcInteractionAnim; } set { value = playerNpcInteractionAnim; } }
    public override Transform targetPosition { get { return target; } set { value = target; } }
    bool canBuyWeaponEffect;
    int objectGold;
    void Awake()
    {
        GameObject[] npcButtons = GameObject.FindGameObjectsWithTag("NPCMarketUI");
        foreach (var item in npcButtons)
        {
            
            item.GetComponent<Button>().onClick.AddListener(Buy);
        }
        MarketUI.SetActive(false);
    }

    public override void Interaction()
    {
        StartCoroutine(InteractionTimer());
    }
    IEnumerator InteractionTimer()
    {
        yield return StartCoroutine(PlayerAnimationController.Instance.InteractionWithMoveAndAnimation(this, targetPosition));
        UIManager.Instance.MarketOpen(MarketUI);
    }

    public void Buy()
    {
        GameObject marketObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        string goldString = marketObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        int.TryParse(goldString, out objectGold);
        if (GameManager.Instance.gold >= objectGold)
        {
            switch (marketObject.transform.parent.name)
            {
                case "Effects":
                    switch (marketObject.name.Substring(0, marketObject.name.IndexOf("Effect")))
                    {
                        case "Fire":
                            PlayerController.Instance.equipmentWeapon.ChangeWeaponEffect(Weapon.WeaponEffectType.Fire,new Color(191,34,0,255));
                            break;
                        case "Poison":
                            PlayerController.Instance.equipmentWeapon.ChangeWeaponEffect(Weapon.WeaponEffectType.Poison,new Color(16,191,0,255));
                            break;
                        case "Blood":
                            PlayerController.Instance.equipmentWeapon.ChangeWeaponEffect(Weapon.WeaponEffectType.Blood,new Color(191,16,16,255));
                            break;
                    }
                    break;
                case "Passive":
                    switch (marketObject.name.Substring(0, marketObject.name.IndexOf("Boost")))
                    {
                        case "Attack":
                            Player.Instance.BuffPassives(Player.PassiveStatus.Damage);
                            break;
                        case "Health":
                            RectTransform healthBarRect =  UIManager.Instance.hpBar.GetComponent<RectTransform>();
                            healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x+10, healthBarRect.sizeDelta.y);
                            healthBarRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x+5,healthBarRect.anchoredPosition.y);
                            Player.Instance.BuffPassives(Player.PassiveStatus.Health);
                            break;
                        case "Stamina":
                            RectTransform staminaBarRect =  UIManager.Instance.staminaBar.GetComponent<RectTransform>();
                            staminaBarRect.sizeDelta = new Vector2(staminaBarRect.sizeDelta.x+10, staminaBarRect.sizeDelta.y);
                            staminaBarRect.anchoredPosition = new Vector2(staminaBarRect.anchoredPosition.x+5,staminaBarRect.anchoredPosition.y);
                            Player.Instance.BuffPassives(Player.PassiveStatus.Stamina);
                            break;
                    }
                    GameManager.Instance.gold -= objectGold;
                    break;
                case "Pots":
                    switch (marketObject.name.Substring(0, marketObject.name.IndexOf("Pot"))){
                        case "Health":
                            Pots.Instance.healthPotionCount++;
                            UIManager.Instance.healthPotCountText.text =Pots.Instance.healthPotionCount.ToString();
                            break;
                        case "Stamina":
                            Pots.Instance.staminaPotionCount++;
                            UIManager.Instance.staminaPotCountText.text =Pots.Instance.staminaPotionCount.ToString();
                            break;   
                    }
                    GameManager.Instance.gold -= objectGold;
                    break;
            }
            UIManager.Instance.goldText.text = "Gold: " + GameManager.Instance.gold.ToString();
        }

    }
}
