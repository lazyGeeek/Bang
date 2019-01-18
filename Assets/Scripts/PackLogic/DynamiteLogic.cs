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
        base.OnCardClick();

        GlobalVeriables.Instance.Player.AddBuff(this);
        Destroy(CurrentCard.gameObject);
        GlobalVeriables.Instance.CardZone.ShowMessage("You throw dynamite");
    }

    public static void CheckDynamite(Character init, DynamiteLogic dynamite)
    {
        PackAsset cardAsset = PackAndDiscard.Instance.GetRandomCard();
        PackAndDiscard.Instance.Discard(cardAsset);

        init.RemoveBuff(dynamite);

        if (cardAsset.CardSuit == ECardSuit.Spades && (int)cardAsset.CardRating >= 2 && (int)cardAsset.CardRating < 10)
        {
            GlobalVeriables.Instance.audioSource.clip = dynamite.explosionAC;
            GlobalVeriables.Instance.audioSource.Play();
            PackAndDiscard.Instance.Discard(dynamite);

            for (int j = 0; j < 3; ++j)
            {
                if (!init.IsDead)
                    init.Hit();
            }
        }
        else
            init.NextPlayer.AddBuff(dynamite);
    }
}
