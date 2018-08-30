using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BeerAsset")]
public class BeerLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        if (UIElements.Instance.Player.Heal())
        {
            UIElements.Instance.Player.RemoveCardToDiscard(this);
            UIElements.Instance.Player.UsedCard.Add(this);
            Destroy(CurrentCard.gameObject);
        }
        else
            UIElements.Instance.CardZone.ShowMessage("Health is full");
    }
}
