using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public Button EndMoveBttn;
    public Button HandBttn;

    [System.NonSerialized]
    public List<PackAsset> UsedCard = new List<PackAsset>();

    public override void InitiateCharacter()
    {
        base.InitiateCharacter();

        BackRoleImage.gameObject.SetActive(false);
    }

    public override void AddBuff(PackAsset buff)
    {
        base.AddBuff(buff);

        UsedCard.Add(buff);
    }

    public void StartMove()
    {
        ImageOfDesk.color = new Color(0.8676471f, 0.7360041f, 0.0f);
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());

        EndMoveBttn.gameObject.SetActive(true);
        HandBttn.gameObject.SetActive(true);
    }

    public void EndMove()
    {
        if (Hand.Count > Health.Length)
        {
            Actions.ShowPlayerCards();
            GlobalVeriables.GameState = EGameState.DropCards;
        }

        EndMoveBttn.gameObject.SetActive(false);
        HandBttn.gameObject.SetActive(false);

        ImageOfDesk.color = Color.white;

        PlayersMoveQueue.StartNextPlayer();
    }
}
