using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreLogic : MonoBehaviour
{
    private static int position = -1;
    private static List<PackAsset> randomCards = new List<PackAsset>();

    public static void Store(Character init)
    {
        if (!UIElements.Instance.Player.IsDead)
            randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        for (int i = 0; i < UIElements.Instance.Enemies.Length; ++i)
        {
            if (UIElements.Instance.Enemies[i] == init)
                position = i;

            if (!UIElements.Instance.Enemies[i].IsDead)
                randomCards.Add(PackAndDiscard.Instance.GetRandomCard());
        }

        if (position != -1)
        {
            for (int i = position; i < UIElements.Instance.Enemies.Length; ++i)
            {
                //Place for AI
            }
        }

        ShowCards.Instance.ShowCardSpawn();
        ShowCards.Instance.ClearCardSpawn();

        foreach (PackAsset randomCard in randomCards)
        {
            Button card = ShowCards.Instance.cardSpawn.AddComponent<Button>();
            card.gameObject.AddComponent<Pack>();
            card.GetComponent<Pack>().CurrentCard = randomCard;
            card.onClick.AddListener(card.GetComponent<Pack>().GetStoreCard);
        }
    }

    public static void ContinueStore(PackAsset ChoosenCard)
    {
        randomCards.Remove(ChoosenCard);

        for (int i = position; i < UIElements.Instance.Enemies.Length; ++i)
        {
            //Place for AI
        }
    }
}
