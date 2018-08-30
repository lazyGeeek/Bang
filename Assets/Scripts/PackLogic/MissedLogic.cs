using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/MissedAsset")]
public class MissedLogic : PackAsset
{
    public override void OnCardClick()
    {
        if (GlobalVeriable.GameState == EGameState.Defense)
        {
            UIElements.Instance.Player.RemoveCardToDiscard(this);
            UIElements.Instance.CardZone.Close();
        }
        else
            UIElements.Instance.CardZone.ShowMessage("You arent under attack");
    }
}
