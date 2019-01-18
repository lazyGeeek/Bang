using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BeerAsset")]
public class BeerLogic : PackAsset
{
    public override void OnCardClick()
    {
        /*if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            GlobalVeriables.Instance.Player.Hand.Remove(this);
            PackAndDiscard.Instance.Discard(this);
            Destroy(CurrentCard.gameObject);
            return;
        }*/
       // base.OnCardClick();

        switch (GlobalVeriables.GameState)
        {
            case EGameState.DropCards:
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                Destroy(CurrentCard.gameObject);
                break;

            case EGameState.Defense:
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                GlobalVeriables.GameState = EGameState.Move;
                GlobalVeriables.Instance.CardZone.Close();
                break;

            case EGameState.Move:
                if (GlobalVeriables.Instance.Player.Heal())
                {
                    GlobalVeriables.Instance.Player.Hand.Remove(this);
                    PackAndDiscard.Instance.Discard(this);
                    GlobalVeriables.Instance.Player.UsedCard.Add(this);
                    Destroy(CurrentCard.gameObject);
                }
                else
                    GlobalVeriables.Instance.CardZone.ShowMessage("Your health is full");
                break;

            default:
                Debug.Log("Unknown game state");
                break;
        }

        /*if (GlobalVeriables.Instance.Player.Heal())
        {
            GlobalVeriables.Instance.Player.Hand.Remove(this);
            PackAndDiscard.Instance.Discard(this);

            if (GlobalVeriables.CurrentPlayer == GlobalVeriables.Instance.Player)
                GlobalVeriables.Instance.Player.UsedCard.Add(this);

            Destroy(CurrentCard.gameObject);
        }
        else
            GlobalVeriables.Instance.CardZone.ShowMessage("Your health is full");*/
    }
}
