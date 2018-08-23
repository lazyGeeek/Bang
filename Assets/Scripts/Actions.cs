using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public static Actions Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public static int GetScope(PackAsset s)
    {
        string name = s.PackSprite.name;

        if (s.CardName == ECardName.Colt) return 2;
        else if (s.CardName == ECardName.Remington) return 3;
        else if (s.CardName == ECardName.Carabine) return 4;
        else if (s.CardName == ECardName.Volcano) return 5;
        return 1;
    }

    public void ShowPlayerCards()
    {
        ShowCards.Instance.ShowCardSpawn();

        foreach (PackAsset card in UIElements.Instance.Player.Hand)
        {
            Button cardButton = Instantiate(card.firstStageButton, ShowCards.Instance.cardSpawn.transform);
            cardButton.image.sprite = card.PackSprite;
            cardButton.GetComponent<Pack>().CurrentCard = card;
        }
    }

    //If barrel work return true
    public static bool CheckBarrel(Character player)
    {
        ShowCards.Instance.ShowCardSpawn();

        Image playerImage = ShowCards.Instance.cardSpawn.AddComponent<Image>();
        playerImage.sprite = player.CharacterInfo.CharacterSprite;

        PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
        Image randomCardImage = ShowCards.Instance.cardSpawn.AddComponent<Image>();
        randomCardImage.sprite = randomCard.PackSprite;

        if (randomCard.CardSuit == ECardSuit.Hearts)
            return true;
        else
            return false;

    }

    public static void Duel(Character initPlayer, Character victimPlayer)
    {
        ShowCards.Instance.ShowCardSpawn();

        int playerBangCount = initPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang")).Count;
        int enemyBangCount = victimPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang")).Count;

        Image initPlayerImage = ShowCards.Instance.cardSpawn.AddComponent<Image>();
        initPlayerImage.sprite = initPlayer.CharacterInfo.CharacterSprite;

        foreach (PackAsset card in initPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang")))
        {
            Image initCard = ShowCards.Instance.cardSpawn.AddComponent<Image>();
            initCard.sprite = card.PackSprite;
        }

        Image victimImage = ShowCards.Instance.cardSpawn.AddComponent<Image>();
        victimImage.sprite = victimPlayer.CharacterInfo.CharacterSprite;

        foreach (PackAsset card in victimPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang")))
        {
            Image victimCard = ShowCards.Instance.cardSpawn.AddComponent<Image>();
            victimCard.sprite = card.PackSprite;
        }

        if (playerBangCount < enemyBangCount)
            initPlayer.Hit(victimPlayer);
        else
            victimPlayer.Hit(initPlayer);
    }

    /*public static void Indians(Character init)
    {
        if (UIElements.Instance.Player != init && UIElements.Instance.Player.Hand.FindAll(l => l.PackSprite.name.Contains("bang")).Count > 0)
            UIElements.Instance.Player.Hit();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy != init && enemy.Hand.FindAll(l => l.PackSprite.name.Contains("bang")).Count > 0)
                enemy.Hit();
        }
    }*/
}
