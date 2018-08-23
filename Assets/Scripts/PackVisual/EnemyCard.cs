using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCard : MonoBehaviour
{
    public void ShowEnemyCard()
    {
        if (GlobalVeriable.CurrentEnemy.Hand.Count < 1)
        {
            ShowCards.Instance.ShowMessage("This player don't have any available card");
            return;
        }

        ShowCards.Instance.ClearCardSpawn();

        foreach (PackAsset enemyCard in GlobalVeriable.CurrentEnemy.Hand)
        {
            Button card = Instantiate(GlobalVeriable.CurrentCard.thirdStageButton, ShowCards.Instance.cardSpawn.transform);
        }

        UIElements.Instance.Player.RemoveCardFromHand(GlobalVeriable.CurrentCard);
        PackAndDiscard.Instance.Discard(GlobalVeriable.CurrentCard);
    }

    public void DropEnemyCard()
    {
        GlobalVeriable.CurrentEnemy.RemoveCardToDiscard(GlobalVeriable.CurrentCard);
        ShowCards.Instance.ClearCardSpawn();
        Actions.Instance.ShowPlayerCards();
    }

    public void GetEnemyCard()
    {
        GlobalVeriable.CurrentEnemy.RemoveCardFromHand(GlobalVeriable.CurrentCard);
        UIElements.Instance.Player.AddCardToHand(GlobalVeriable.CurrentCard);
        ShowCards.Instance.ClearCardSpawn();
        Actions.Instance.ShowPlayerCards();
    }
}
