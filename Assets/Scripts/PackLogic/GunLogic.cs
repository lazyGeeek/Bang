using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/GunAsset")]
public class GunLogic : PackAsset
{
    public override void OnCardClick()
    {
        UIElements.Instance.Player.RemoveCardFromHand(this);

        if (UIElements.Instance.Player.Weapon != null)
            Actions.CreateCard(UIElements.Instance.Player.Weapon);

        UIElements.Instance.Player.Weapon = this;
        Destroy(CurrentCard.gameObject);
    }
}
