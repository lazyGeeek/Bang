using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDefense : MonoBehaviour
{
    public static void Defense(Bot victim, Character enemy)
    {
        PackAsset missed = victim.Hand.Find(card => card.CardName == ECardName.Missed);

        if (missed != null)
        {
            victim.Hand.Remove(missed);

            if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
                GlobalVeriables.Instance.CardZone.ShowMessage("You missed!");

            return;
        }

        PackAsset barrel = victim.Buffs.Find(asset => asset.CardName == ECardName.Barrel);

        if (barrel != null)
        {
            victim.RemoveBuff(barrel);
            if (BarrelLogic.CheckBarrel(victim))
            {
                if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
                    GlobalVeriables.Instance.CardZone.ShowMessage("Enemy behind barrel");
                return;
            }
        }

        if (victim.CurrentHealth == 1)
        {
            PackAsset beer = victim.Hand.Find(card => card.CardName == ECardName.Beer);

            if (beer != null)
            {
                victim.Hand.Remove(beer);
                if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
                    GlobalVeriables.Instance.CardZone.ShowMessage("Enemy heal!");
                return;
            }
        }

        if (!victim.botEnemies.Contains(enemy))
            victim.botEnemies.Add(enemy);

        victim.Hit(enemy);
        victim.ShowBulletHole();
    }
}