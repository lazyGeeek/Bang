using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDefense : MonoBehaviour
{
    public static void Defense(Character victim, Character enemy)
    {
        if (victim.CharacterInfo.Name == ECharacterName.Jourdonnais)
        {
            if (Actions.CheckBarrel(victim))
            {
                ShowCards.Instance.ShowMessage("Enemy behind barrel");
                return;
            }
        }
        else if (victim.Hand.Find(card => card.CardName == ECardName.Missed))
        {
            victim.RemoveCardFromHand(victim.Hand.Find(card => card.CardName == ECardName.Missed));
            ShowCards.Instance.ShowMessage("You missed!");
            return;
        }
        else if (victim.Hand.Find(card => card.CardName == ECardName.Beer))
        {
            victim.RemoveCardFromHand(victim.Hand.Find(card => card.CardName == ECardName.Beer));
            ShowCards.Instance.ShowMessage("Enemy heal!");
            return;
        }
        else if (victim.CharacterInfo.Name == ECharacterName.CalamityJanet && victim.Hand.Find(card => card.CardName == ECardName.Bang))
        {
            victim.RemoveCardFromHand(victim.Hand.Find(card => card.CardName == ECardName.Bang));
            ShowCards.Instance.ShowMessage("You missed!");
            return;
        }
        else if (victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel))
        {
            victim.RemoveBuff(victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel));
            if (Actions.CheckBarrel(victim))
            {
                ShowCards.Instance.ShowMessage("Enemy behind barrel");
                return;
            }
        }

        victim.Hit(enemy);
        victim.ShowBulletHole();
    }
}
