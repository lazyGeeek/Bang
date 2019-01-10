using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/DynamiteAsset")]
public class DynamiteLogic : PackAsset
{
    [SerializeField]
    private AudioClip explosionAC;

    public override void OnCardClick()
    {
        UIElements.Instance.Player.AddBuff(this);
        Destroy(CurrentCard.gameObject);
        UIElements.Instance.CardZone.ShowMessage("You throw dynamite");
    }

    public static void CheckDynamite(Character init, DynamiteLogic dynamite)
    {
        PackAsset cardAsset = PackAndDiscard.Instance.GetRandomCard();
        PackAndDiscard.Instance.Discard(cardAsset);

        init.RemoveBuff(dynamite);

        if (cardAsset.CardSuit == ECardSuit.Spades && (int)cardAsset.CardRating >= 2 && (int)cardAsset.CardRating < 10)
        {
            UIElements.Instance.audioSource.clip = dynamite.explosionAC;
            UIElements.Instance.audioSource.Play();

            for (int j = 0; j < 3; ++j)
            {
                if (!init.IsDead)
                    init.Hit();
            }
        }
        else
        {
            Character nextPlayer;
            int nextPlayerIndex = UIElements.Instance.Enemies.FindIndex(player => player == init);
            
            if (nextPlayerIndex == -1)
                nextPlayer = UIElements.Instance.Enemies[0];
            else if (nextPlayerIndex == 3)
                nextPlayer = UIElements.Instance.Player;
            else
                nextPlayer = UIElements.Instance.Enemies[nextPlayerIndex + 1];

            nextPlayer.AddBuff(dynamite);
        }
    }
}
