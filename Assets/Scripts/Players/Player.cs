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

    protected override void Death(Character enemy)
    {
        if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
            GlobalVeriables.Instance.CardZone.Close();

        switch (RoleInfo.Role)
        {
            case ERole.Sheriff:
                GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("You are dead! Without you, the town is mired in cruelty, civilians unfortunately did not survive");
                break;
            case ERole.Assistant:
                GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("You are dead! Without you the sheriff failed and the city was destroyed");
                break;
            case ERole.Renegade:
                GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("You are dead! The sheriff defeated the bandits, and you were quickly forgotten");
                break;
            case ERole.Bandit:
                GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("You are dead! Your gang was caught or killed, and your nameless corpse is not suitable even as a toilet for wild dogs");
                break;
            default:
                Debug.Log("Unknown role");
                break;
        }
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

        StartCoroutine(PlayersMoveQueue.StartNextPlayer());
    }
}
