using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/MissedAsset")]
public class MissedLogic : PackAsset
{
    public override void OnCardClick()
    {
        switch(GlobalVeriables.GameState)
        {
            case EGameState.DropCards:
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                Destroy(CurrentCard.gameObject);
                break;

            case EGameState.Defense:
                GlobalVeriables.GameState = EGameState.Move;
                GlobalVeriables.Instance.Player.Hand.Remove(this);
                PackAndDiscard.Instance.Discard(this);
                GlobalVeriables.Instance.CardZone.Close();
                break;

            case EGameState.Move:
                GlobalVeriables.Instance.CardZone.ShowMessage("You arent under attack");
                break;

            default:
                Debug.Log("Unknown game state");
                break;
        }
    }
}
