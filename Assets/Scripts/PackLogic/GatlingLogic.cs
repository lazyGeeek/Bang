using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/GatlingAsset")]
public class GatlingLogic : PackAsset
{
    public AudioClip gatlingAudio;

    public override void OnCardClick()
    {
        base.OnCardClick();

        Gatling(UIElements.Instance.Player, this);
        Destroy(CurrentCard.gameObject);
    }

    public static void Gatling(Character initPlayer, GatlingLogic currentCard)
    {
        initPlayer.Hand.Remove(currentCard);
        initPlayer.UsedCard.Add(currentCard);
        UIElements.Instance.audioSource.clip = currentCard.gatlingAudio;
        UIElements.Instance.audioSource.Play();

        if (UIElements.Instance.Player != initPlayer && !UIElements.Instance.Player.IsDead)
        {
            List<PackAsset> defenseCard = new List<PackAsset>();
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Missed));
            defenseCard.AddRange(UIElements.Instance.Player.Buffs.FindAll(card => card.CardName == ECardName.Barrel));

            if (UIElements.Instance.Player.CurrentHealth == 1)
                defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Beer));

            if (defenseCard.Count == 0)
            {
                UIElements.Instance.Player.Hit();
                UIElements.Instance.Player.ShowBulletHole();
            }
            else
            {
                GlobalVeriables.GameState = EGameState.Defense;
                UIElements.Instance.CardZone.ShowCardSpawn();
                UIElements.Instance.CardZone.ClearCardSpawn();
                UIElements.Instance.CardZone.ShowPermanentMessage("Pick up card for defense");

                foreach (PackAsset card in defenseCard)
                    Actions.CreateCard(card);
            }
        }

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy != initPlayer && !enemy.IsDead)
                AIDefense.Defense(enemy, initPlayer);
        }

        Actions.Wait(UIElements.Instance.audioSource.time);
    }
}
