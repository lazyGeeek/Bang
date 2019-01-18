using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BarrelAsset")]
public class BarrelLogic : PackAsset
{
    public override void OnCardClick()
    {
        switch (GlobalVeriables.GameState)
        {
            case EGameState.DropCards:
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                Destroy(CurrentCard.gameObject);
                break;

            case EGameState.Defense:
                _CheckBarrel();
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                Destroy(CurrentCard);
                break;

            case EGameState.Move:
                if (!GlobalVeriables.Instance.Player.Buffs.Contains(this))
                {
                    GlobalVeriables.Instance.Player.AddBuff(this);
                    GlobalVeriables.Instance.Player.Hand.Remove(this);
                }
                else
                    GlobalVeriables.Instance.CardZone.ShowMessage("You already have this buff");
                break;

            default:
                Debug.Log("Add current state");
                break;
        }
    }

    private void _CheckBarrel()
    {
        GlobalVeriables.Instance.CardZone.ShowCardSpawn(true, false);

        _CreateCard().sprite = GlobalVeriables.Instance.Player.CharacterImage.sprite;

        PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
        _CreateCard().sprite = GlobalVeriables.Instance.Player.CharacterImage.sprite;
        PackAndDiscard.Instance.Discard(randomCard);

        if (randomCard.CardSuit == ECardSuit.Hearts)
        {
            GlobalVeriables.Instance.CardZone.ShowPermanentMessage("You hide behind barrel");
            GlobalVeriables.GameState = EGameState.Move;
        }
        else
            GlobalVeriables.Instance.CardZone.ShowPermanentMessage("You too big to hide behind barrel");
    }

    private static Image _CreateCard()
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Image cardImage = cardObject.AddComponent<Image>();
        return cardImage;
    }

    public static bool CheckBarrel(Character player)
    {
        PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
        PackAndDiscard.Instance.Discard(randomCard);

        if (randomCard.CardSuit != ECardSuit.Hearts)
        {
            player.Hit();
            return false;
        }

        return true;
    }
}
