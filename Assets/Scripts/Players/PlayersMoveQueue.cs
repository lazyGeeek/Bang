using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayersMoveQueue
{
    public static IEnumerator StartNextPlayer()
    {
        GlobalVeriables.CurrentPlayer = GlobalVeriables.CurrentPlayer.NextPlayer;

        GlobalVeriables.CurrentPlayer.StartCoroutine(GlobalVeriables.Instance.CurrentPlayerZone.ShowPlayer(GlobalVeriables.CurrentPlayer.CharacterImage));

        yield return new WaitWhile(() => GlobalVeriables.Instance.CurrentPlayerZone.isActiveAndEnabled);

        PackAsset dynamite = GlobalVeriables.CurrentPlayer.Buffs.Find(card => card.CardName == ECardName.Dynamite);
        
        if (dynamite != null)
            DynamiteLogic.CheckDynamite(GlobalVeriables.CurrentPlayer, (DynamiteLogic)dynamite);

        yield return new WaitWhile(() => GlobalVeriables.Instance.DeadMessageZone.isActiveAndEnabled);

        PackAsset jail = GlobalVeriables.CurrentPlayer.Buffs.Find(card => card.CardName == ECardName.Jail);
        bool canMove = true;

        if (jail != null)
        {
            GlobalVeriables.CurrentPlayer.RemoveBuff(jail);
            PackAndDiscard.Instance.Discard(jail);

            canMove = _CheckJail();
        }

        if (canMove)
        {
            if (GlobalVeriables.CurrentPlayer == GlobalVeriables.Instance.Player)
            {
                ((Player)GlobalVeriables.CurrentPlayer).StartMove();
                yield return new WaitUntil(() => ((Player)GlobalVeriables.CurrentPlayer).EndMoveBttn.isActiveAndEnabled);
            }
            else
            {
                GlobalVeriables.CurrentPlayer.StartCoroutine(((Bot)GlobalVeriables.CurrentPlayer).StartMove());
                yield return new WaitUntil(() => ((Bot)GlobalVeriables.CurrentPlayer).UsingCards.activeSelf);
            }
        }
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
