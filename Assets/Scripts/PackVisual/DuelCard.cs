using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelCard : MonoBehaviour
{
    public void StartDuel()
    {
        UIElements.Instance.Player.RemoveCardFromHand(GlobalVeriable.CurrentCard);
        PackAndDiscard.Instance.Discard(GlobalVeriable.CurrentCard);
        ShowCards.Instance.ClearCardSpawn();
        Actions.Duel(UIElements.Instance.Player, GlobalVeriable.CurrentEnemy);
    }
}
