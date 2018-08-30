using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDefense : MonoBehaviour
{
    public static void Defense(Character victim, Character enemy)
    {
        if (victim.Hand.Find(card => card.CardName == ECardName.Missed))
        {
            victim.RemoveCardFromHand(victim.Hand.Find(card => card.CardName == ECardName.Missed));
            UIElements.Instance.CardZone.ShowMessage("You missed!");
            return;
        }
        else if (victim.Hand.Find(card => card.CardName == ECardName.Beer))
        {
            victim.RemoveCardFromHand(victim.Hand.Find(card => card.CardName == ECardName.Beer));
            UIElements.Instance.CardZone.ShowMessage("Enemy heal!");
            return;
        }
        else if (victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel))
        {
            victim.RemoveBuff(victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel));
            if (BarrelLogic.CheckBarrel(victim))
            {
                UIElements.Instance.CardZone.ShowMessage("Enemy behind barrel");
                return;
            }
        }

        victim.Hit(enemy);
        victim.ShowBulletHole();
    }
}
