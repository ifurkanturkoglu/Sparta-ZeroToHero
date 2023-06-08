using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMarket : Interactable
{
    [SerializeField] AnimatorOverrideController playerNpcInteractionAnim;
    [SerializeField] Transform target;
    [SerializeField] GameObject MarketUI;
    public override AnimatorOverrideController animatorOverrideController { get { return playerNpcInteractionAnim; } set { value = playerNpcInteractionAnim; } }
    public override Transform targetPosition { get { return target; } set { value = target; } }
    bool canBuyWeaponEffect;
    void Awake()
    {
        GameObject[] npcButtons = GameObject.FindGameObjectsWithTag("NPCMarketUI");
        foreach (var item in npcButtons)
        {
            item.GetComponent<Button>().onClick.AddListener(Buy);
        }
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
        print(marketObject.name);
        int objectGold = int.Parse(marketObject.transform.GetChild(1).GetComponent<Text>().text);
        if (GameManager.Instance.gold >= objectGold)
        {
            switch (marketObject.tag)
            {
                case "EffectUI":
                    print("effect: "+marketObject.name);
                    break;
                case "PassiveUI":
                    print("passive: "+marketObject.name);
                    break;
                case "PotUI":
                    print("pot: "+marketObject.name);
                    break;
            }
        }

    }
}
