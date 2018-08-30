using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BuffAsset")]
public class BuffLogic : PackAsset
{
    public override void OnCardClick()
    {
        if (Buff(UIElements.Instance.Player, this))
        {
            UIElements.Instance.Player.RemoveCardToDiscard(this);
            Destroy(CurrentCard.gameObject);
        }
        else
            UIElements.Instance.CardZone.ShowMessage("You already have this buff");
    }

    public static bool Buff(Character player, PackAsset card)
    {
        if (!player.Buffs.Contains(card))
            UIElements.Instance.Player.AddBuff(card);
        else
            return false;

        return true;
    }
}
