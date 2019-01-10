using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDefense : MonoBehaviour
{
    public static void Defense(Character victim, Character enemy)
    {
        PackAsset missed = victim.Hand.Find(card => card.CardName == ECardName.Missed);

        if (missed != null)
        {
            victim.Hand.Remove(missed);

            if (UIElements.Instance.CardZone.isActiveAndEnabled)
                UIElements.Instance.CardZone.ShowMessage("You missed!");

            return;
        }

        PackAsset barrel = victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel);

        if (barrel != null)
        {
            victim.RemoveBuff(barrel);
            if (BarrelLogic.CheckBarrel(victim))
            {
                if (UIElements.Instance.CardZone.isActiveAndEnabled)
                    UIElements.Instance.CardZone.ShowMessage("Enemy behind barrel");
                return;
            }
        }

        PackAsset beer = victim.Hand.Find(card => card.CardName == ECardName.Beer);

        if (beer != null && victim.CurrentHealth == 1)
        {
            victim.Hand.Remove(beer);
            if (UIElements.Instance.CardZone.isActiveAndEnabled)
                UIElements.Instance.CardZone.ShowMessage("Enemy heal!");
            return;
        }

        victim.Hit(enemy);
        victim.ShowBulletHole();
    }
}