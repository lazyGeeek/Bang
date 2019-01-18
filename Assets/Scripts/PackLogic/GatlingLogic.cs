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

        _Gatling(GlobalVeriables.Instance.Player, this);
        GlobalVeriables.Instance.Player.UsedCard.Add(this);
        Destroy(CurrentCard.gameObject);
    }

    public static IEnumerator Gatling(Bot init, GatlingLogic currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        GlobalVeriables.Instance.audioSource.clip = currentCard.gatlingAudio;
        GlobalVeriables.Instance.audioSource.Play();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (enemy != init && !enemy.IsDead)
                AIDefense.Defense(enemy, init);
        }

        List<PackAsset> defenseCard = new List<PackAsset>();
        defenseCard.AddRange(GlobalVeriables.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Missed));
        defenseCard.AddRange(GlobalVeriables.Instance.Player.Buffs.FindAll(card => card.CardName == ECardName.Barrel));

        if (GlobalVeriables.Instance.Player.CurrentHealth == 1)
            defenseCard.AddRange(GlobalVeriables.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Beer));

        if (defenseCard.Count == 0)
        {
            GlobalVeriables.Instance.Player.Hit();
            GlobalVeriables.Instance.Player.ShowBulletHole();
        }
        else
        {
            GlobalVeriables.GameState = EGameState.Defense;
            GlobalVeriables.Instance.CardZone.ShowCardSpawn(true, false);
            GlobalVeriables.Instance.CardZone.ShowPermanentMessage("Pick up card for defense");

            foreach (PackAsset card in defenseCard)
                Actions.CreateCard(card);
        }

        yield return new WaitForSeconds(GlobalVeriables.Instance.audioSource.time);
    }

    private static void _Gatling(Player initPlayer, GatlingLogic currentCard)
    {
        initPlayer.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        GlobalVeriables.Instance.audioSource.clip = currentCard.gatlingAudio;
        GlobalVeriables.Instance.audioSource.Play();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (!enemy.IsDead)
                AIDefense.Defense(enemy, initPlayer);
        }
    }
}
