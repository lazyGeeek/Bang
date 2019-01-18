using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/GetCardsAsset")]
public class GetCardsLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        foreach (PackAsset card in StageCoach(GlobalVeriables.Instance.Player, this))
            Actions.CreateCard(card);

        GlobalVeriables.Instance.Player.UsedCard.Add(this);
        Destroy(CurrentCard.gameObject);
    }

    public static PackAsset[] StageCoach(Character init, PackAsset currentCard)
    {
        int count = currentCard.CardName == ECardName.WellsFargo ? 3 : 2;
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);

        PackAsset[] newCards = new PackAsset[count];

        for (int i = 0; i < count; ++i)
        {
            PackAsset card = PackAndDiscard.Instance.GetRandomCard();
            init.Hand.Add(card);
            newCards[i] = card;
        }

        return newCards;
    }
}
