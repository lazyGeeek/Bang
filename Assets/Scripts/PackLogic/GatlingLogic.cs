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
        Destroy(CurrentCard);
    }

    public static void Gatling(Character initPlayer, GatlingLogic currentCard)
    {
        UIElements.Instance.Player.RemoveCardToDiscard(currentCard);
        UIElements.Instance.Player.UsedCard.Add(currentCard);
        UIElements.Instance.audioSource.clip = currentCard.gatlingAudio;
        UIElements.Instance.audioSource.Play();

        if (UIElements.Instance.Player.CharacterImage.sprite != initPlayer.CharacterImage.sprite && !UIElements.Instance.Player.IsDead)
        {
            List<PackAsset> defenseCard = new List<PackAsset>();
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Missed));
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Beer));
            defenseCard.AddRange(UIElements.Instance.Player.Buffs.FindAll(card => card.CardName == ECardName.Barrel));

            if (defenseCard.Count < 1)
            {
                UIElements.Instance.Player.Hit();
                UIElements.Instance.Player.ShowBulletHole();
            }
            else
            {
                UIElements.Instance.CardZone.ShowCardSpawn();
                UIElements.Instance.CardZone.ClearCardSpawn();
                GlobalVeriable.GameState = EGameState.Defense;

                foreach (PackAsset card in defenseCard)
                    Actions.CreateCard(card);
            }
        }

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy.CharacterImage.sprite != initPlayer.CharacterImage.sprite && !enemy.IsDead)
                AIDefense.Defense(enemy, initPlayer);
        }

        Actions.Wait(UIElements.Instance.audioSource.time);
    }
}
