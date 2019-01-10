using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayersMoveQueue
{
    public static void StartNextPlayer()
    {
        GlobalVeriables.CurrentPlayer = GlobalVeriables.CurrentPlayer.NextPlayer;

        Actions.Instance.StartCoroutine(UIElements.Instance.CurrentPlayerZone.ShowPlayer(GlobalVeriables.CurrentPlayer.CharacterImage));

        PackAsset dynamite = GlobalVeriables.CurrentPlayer.Buffs.Find(card => card.CardName == ECardName.Dynamite);
        
        if (dynamite != null)
            DynamiteLogic.CheckDynamite(GlobalVeriables.CurrentPlayer, (DynamiteLogic)dynamite);

        PackAsset jail = GlobalVeriables.CurrentPlayer.Buffs.Find(card => card.CardName == ECardName.Jail);
        bool canMove = true;

        if (jail != null)
        {
            GlobalVeriables.CurrentPlayer.RemoveBuff(jail);
            PackAndDiscard.Instance.Discard(jail);

            canMove = _CheckJail();
        }

        if (canMove)
            GlobalVeriables.CurrentPlayer.StartMove();
    }

    private static bool _CheckJail()
    {
        PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
        PackAndDiscard.Instance.Discard(randomCard);

        if (randomCard.CardSuit == ECardSuit.Hearts)
            return true;

        return false;
    }
}
