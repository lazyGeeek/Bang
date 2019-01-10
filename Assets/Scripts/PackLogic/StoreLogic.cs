using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/StoreAsset")]
public class StoreLogic : PackAsset
{
    private static int position = -1;
    private static List<PackAsset> randomCards = new List<PackAsset>();

    public override void OnCardClick()
    {
        base.OnCardClick();

        UIElements.Instance.CardZone.ClearCardSpawn();
        Store(UIElements.Instance.Player, this);
    }

    public static void Store(Character init, PackAsset currentCard)
    {
        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);

        if (!UIElements.Instance.Player.IsDead)
            randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        for (int i = 0; i < UIElements.Instance.Enemies.Count; ++i)
        {
            if (UIElements.Instance.Enemies[i] == init)
                position = i;

            if (!UIElements.Instance.Enemies[i].IsDead)
                randomCards.Add(PackAndDiscard.Instance.GetRandomCard());
        }

        if (position != -1)
        {
            for (int i = position; i < UIElements.Instance.Enemies.Count; ++i)
            {
                PackAsset enemyChosenCard = AIPickUpCard.PickUpCard(UIElements.Instance.Enemies[i], randomCards);
                randomCards.Remove(enemyChosenCard);
                UIElements.Instance.Enemies[i].AddCardToHand(enemyChosenCard);
            }
        }

        UIElements.Instance.CardZone.ShowCardSpawn();
        UIElements.Instance.CardZone.ClearCardSpawn();
        UIElements.Instance.CardZone.closeButton.gameObject.SetActive(false);
        UIElements.Instance.CardZone.dropCardButton.gameObject.SetActive(false);

        foreach (PackAsset randomCard in randomCards)
        {
            Button card = Actions.CreateCard(randomCard);
            card.onClick.AddListener(delegate { ContinueStore(randomCard); });
        }
    }

    public static void ContinueStore(PackAsset ChoosenCard)
    {
        randomCards.Remove(ChoosenCard);
        UIElements.Instance.Player.AddCardToHand(ChoosenCard);
        
        for (int i = 0; i < position; ++i)
        {
            PackAsset enemyChosenCard = AIPickUpCard.PickUpCard(UIElements.Instance.Enemies[i], randomCards);
            randomCards.Remove(enemyChosenCard);
            UIElements.Instance.Enemies[i].AddCardToHand(enemyChosenCard);
        }

        foreach (PackAsset randomCard in randomCards)
            PackAndDiscard.Instance.Discard(randomCard);

        if (GlobalVeriables.CurrentPlayer == UIElements.Instance.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();
            Actions.Instance.ShowPlayerCards();
        }
        else
            UIElements.Instance.CardZone.Close();
    }
}
