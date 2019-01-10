using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/JailAsset")]
public class JailLogic : PackAsset
{
    public override void OnCardClick()
    {
        UIElements.Instance.CardZone.ClearCardSpawn();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            Button enemyCard = Actions.CreateCard(enemy);
            enemyCard.onClick.AddListener(delegate { Jail(UIElements.Instance.Player, enemy, this); });
        }
    }

    public static void Jail(Character init, Character defendant, PackAsset currentCard)
    {
        if (defendant.RoleInfo.Role == ERole.Sheriff)
        {
            if (UIElements.Instance.CardZone.gameObject.activeSelf)
                UIElements.Instance.CardZone.ShowMessage("You cant put sheriff in jail");
            return;
        }
        else if (defendant.InJail)
        {
            if (UIElements.Instance.CardZone.gameObject.activeSelf)
                UIElements.Instance.CardZone.ShowMessage("He is already in jail");
            return;
        }
        
        init.Hand.Remove(currentCard);
        init.UsedCard.Add(currentCard);
        defendant.InJail = true;
        defendant.AddBuff(currentCard);
        
        if (init.Type == ECharacterType.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();
            Actions.Instance.ShowPlayerCards();
        }
            
    }
}
