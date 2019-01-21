using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/JailAsset")]
public class JailLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (GlobalVeriables.CurrentSheriff != enemy && !enemy.InJail)
            {
                Button enemyCard = Actions.CreateCard(enemy);
                enemyCard.onClick.AddListener(delegate { _Jail(GlobalVeriables.Instance.Player, enemy, this); });
            }
        }
    }

    private static void _Jail(Player init, Bot defendant, PackAsset currentCard)
    {
        init.Hand.Remove(currentCard);
        init.UsedCard.Add(currentCard);
        defendant.InJail = true;
        defendant.AddBuff(currentCard);

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        Actions.ShowPlayerCards();
    }

    public static void Jail(Character init, Character defendant, PackAsset currentCard)
    {
        if (defendant.RoleInfo.Role == ERole.Sheriff || defendant.InJail)
            return;
        
        init.Hand.Remove(currentCard);
        defendant.InJail = true;
        defendant.AddBuff(currentCard);
    }
}
