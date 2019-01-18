using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/GunAsset")]
public class GunLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        GlobalVeriables.Instance.Player.Hand.Remove(this);
        PackAndDiscard.Instance.Discard(this);

        if (GlobalVeriables.Instance.Player.Weapon != null)
            Actions.CreateCard(GlobalVeriables.Instance.Player.Weapon);

        GlobalVeriables.Instance.Player.Weapon = this;
        Destroy(CurrentCard.gameObject);
    }
}
