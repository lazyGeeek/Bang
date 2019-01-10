using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BarrelAsset")]
public class BarrelLogic : PackAsset
{
    public override void OnCardClick()
    {
        if (GlobalVeriables.GameState == EGameState.Defense)
        {
            CheckBarrel(UIElements.Instance.Player);
            UIElements.Instance.Player.RemoveCardToDiscard(this);
        }
        else
        {
            if (UIElements.Instance.Player.Buffs.Contains(this))
            {
                UIElements.Instance.Player.AddBuff(this);
                UIElements.Instance.Player.Hand.Remove(this);
            }
            else
                UIElements.Instance.CardZone.ShowMessage("You don't defense");
        }
    }

    public static bool CheckBarrel(Character player)
    {
        UIElements.Instance.CardZone.ShowCardSpawn();

        Image playerImage = UIElements.Instance.CardZone.cardSpawn.AddComponent<Image>();
        playerImage.sprite = player.CharacterImage.sprite;

        PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
        Image randomCardImage = UIElements.Instance.CardZone.cardSpawn.AddComponent<Image>();
        randomCardImage.sprite = randomCard.PackSprite;

        PackAndDiscard.Instance.Discard(randomCard);

        if (randomCard.CardSuit != ECardSuit.Hearts)
        {
            player.Hit();
            return false;
        }
        else
            return true;
    }
}
