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

        if (CardName == ECardName.Stagecoach)
            StageCoach(UIElements.Instance.Player, this);
        else if (CardName == ECardName.WellsFargo)
            StageCoach(UIElements.Instance.Player, this);
        Destroy(CurrentCard.gameObject);
    }

    public static void StageCoach(Character init, PackAsset currentCard)
    {
        int count = currentCard.CardName == ECardName.WellsFargo ? 3 : 2;
        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);

        List<PackAsset> newCards = new List<PackAsset>();

        for (int i = 0; i < count; ++i)
            newCards.Add(PackAndDiscard.Instance.GetRandomCard());

        foreach (PackAsset newCard in newCards)
        {
            init.AddCardToHand(newCard);

            if (init.Type == ECharacterType.Player)
            {
                Button card = Actions.CreateCard(newCard);
                card.onClick.AddListener(newCard.OnCardClick);
            }
        }
    }
}
