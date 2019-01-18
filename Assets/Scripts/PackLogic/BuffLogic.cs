using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BuffAsset")]
public class BuffLogic : PackAsset
{
    public override void OnCardClick()
    {
        /*if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            GlobalVeriables.Instance.Player.Hand.Remove(this);
            PackAndDiscard.Instance.Discard(this);
            Destroy(CurrentCard.gameObject);
            return;
        }*/
        base.OnCardClick();

        if (Buff(GlobalVeriables.Instance.Player, this))
            Destroy(CurrentCard.gameObject);
        else
            GlobalVeriables.Instance.CardZone.ShowMessage("You already have this buff");
    }

    public static bool Buff(Character player, PackAsset card)
    {
        if (!player.Buffs.Contains(card))
            GlobalVeriables.Instance.Player.AddBuff(card);
        else
            return false;

        return true;
    }
}
