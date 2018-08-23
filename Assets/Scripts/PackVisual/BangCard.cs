using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangCard : MonoBehaviour
{
    public AudioSource AudioS;

    public void Shoot()
    {
        UIElements.Instance.Player.RemoveCardFromHand(GlobalVeriable.CurrentCard);
        UIElements.Instance.Player.UsedCard.Add(GlobalVeriable.CurrentCard);
        AudioS.Play();
        AIDefense.Defense(GlobalVeriable.CurrentEnemy, UIElements.Instance.Player);
        PackAndDiscard.Instance.Discard(GlobalVeriable.CurrentCard);
        //Actions.Instance.ShowPlayerCards();
    }
}
