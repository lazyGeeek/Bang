using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/StoreAsset")]
public class StoreLogic : PackAsset
{
    private static List<PackAsset> randomCards = new List<PackAsset>();

    public override void OnCardClick()
    {
        base.OnCardClick();
        
        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        _Store(GlobalVeriables.Instance.Player, this);
    }

    private static void _Store(Player init, PackAsset currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        init.UsedCard.Add(currentCard);

        randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        for (int i = 0; i < GlobalVeriables.Instance.Enemies.Count; i++)
            randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        GlobalVeriables.Instance.CardZone.ShowCardSpawn(false, false);

        foreach (PackAsset randomCard in randomCards)
        {
            Button card = Actions.CreateCard(randomCard);
            card.onClick.AddListener(delegate { _GetCardForPlayer(randomCard); });
        }
    }

    private static void _GetCardForPlayer(PackAsset choosenCard)
    {
        randomCards.Remove(choosenCard);
        GlobalVeriables.Instance.Player.Hand.Add(choosenCard);

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        Actions.ShowPlayerCards();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            PackAsset enemyChosenCard = AIPickUpCard.PickUpCard(enemy, randomCards);
            randomCards.Remove(enemyChosenCard);
            enemy.Hand.Add(enemyChosenCard);
        }

        foreach (PackAsset randomCard in randomCards)
            PackAndDiscard.Instance.Discard(randomCard);
    }

    public static void Store(Bot init, PackAsset currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);

        if (!GlobalVeriables.Instance.Player.IsDead)
            randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        for (int i = 0; i < GlobalVeriables.Instance.Enemies.Count; i++)
            randomCards.Add(PackAndDiscard.Instance.GetRandomCard());

        //List<Bot> enemies = new List<Bot>(GlobalVeriables.Instance.Enemies);

        for (int i = GlobalVeriables.Instance.Enemies.FindIndex(pl => pl == init); i < GlobalVeriables.Instance.Enemies.Count; i++)
        {
            PackAsset enemyChosenCard = AIPickUpCard.PickUpCard(GlobalVeriables.Instance.Enemies[i], randomCards);
            randomCards.Remove(enemyChosenCard);
            GlobalVeriables.Instance.Enemies[i].Hand.Add(enemyChosenCard);
        }

        foreach (PackAsset randomCard in randomCards)
        {
            Button card = Actions.CreateCard(randomCard);
            card.onClick.AddListener(delegate { _GetCard(init, randomCard); });
        }
    }

    private static void _GetCard(Bot init, PackAsset choosenCard)
    {
        randomCards.Remove(choosenCard);
        GlobalVeriables.Instance.Player.Hand.Add(choosenCard);

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        GlobalVeriables.Instance.CardZone.Close();

        for (int i = 0; i < GlobalVeriables.Instance.Enemies.FindIndex(pl => pl == init); i++)
        {
            PackAsset enemyChosenCard = AIPickUpCard.PickUpCard(GlobalVeriables.Instance.Enemies[i], randomCards);
            randomCards.Remove(enemyChosenCard);
            GlobalVeriables.Instance.Enemies[i].Hand.Add(enemyChosenCard);
        }

        foreach (PackAsset randomCard in randomCards)
            PackAndDiscard.Instance.Discard(randomCard);
    }
}
