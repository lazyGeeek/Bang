using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPickUpCard : MonoBehaviour
{
    public static PackAsset PickUpCard(Character bot, List<PackAsset> cardList)
    {
        if (cardList.Exists(asset => asset.CardName == ECardName.Bang))
            return cardList.Find(asset => asset.CardName == ECardName.Bang);
        else if (cardList.Exists(asset => asset.CardName == ECardName.Beer))
            return cardList.Find(asset => asset.CardName == ECardName.Beer);
        else if (cardList.Exists(asset => asset.CardName == ECardName.Missed))
            return cardList.Find(asset => asset.CardName == ECardName.Missed);
        else if (cardList.Exists(asset => asset.CardType == ECardType.Act))
        {
            List<PackAsset> actList = cardList.FindAll(asset => asset.CardType == ECardType.Act);
            return actList[Random.Range(0, actList.Count)];
        }
        else
            return cardList[Random.Range(0, cardList.Count)];
    }
}
