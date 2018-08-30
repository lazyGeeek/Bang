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
            UIElements.Instance.CardZone.ShowMessage("You cant put sheriff in jail");
            return;
        }
        else if (!defendant.InJail)
        {
            UIElements.Instance.CardZone.ShowMessage("He already in jail");
            return;
        }

        defendant.PutInJail();

        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);
        
        if (init.Type == ECharacterType.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();
            Actions.Instance.ShowPlayerCards();
        }
            
    }
}
