using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/MissedAsset")]
public class MissedLogic : PackAsset
{
    public override void OnCardClick()
    {
        if (GlobalVeriables.GameState == EGameState.Defense)
        {
            GlobalVeriables.GameState = EGameState.Move;
            UIElements.Instance.Player.RemoveCardToDiscard(this);
            UIElements.Instance.CardZone.Close();
        }
        else if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            UIElements.Instance.Player.RemoveCardToDiscard(this);
            Destroy(CurrentCard.gameObject);
        }
        else
            UIElements.Instance.CardZone.ShowMessage("You arent under attack");
    }
}
