using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/DuelAsset")]
public class DuelLogic : PackAsset
{
    [SerializeField]
    private readonly AudioClip shootAC;

    public override void OnCardClick()
    {
        base.OnCardClick();

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        GlobalVeriables.Instance.audioSource.clip = shootAC;

        GlobalVeriables.Instance.CardZone.ShowPermanentMessage("Pick up someone for duel");

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (!enemy.IsDead)
            {
                Button enemyCard = Actions.CreateCard(enemy);
                enemyCard.onClick.AddListener(delegate { _Duel(GlobalVeriables.Instance.Player, enemy, this); });
            }
        }
    }

    private static void _Duel(Character initPlayer, Character victimPlayer, PackAsset currentCard)
    {
        GlobalVeriables.Instance.CardZone.ShowCardSpawn(true, false);
        initPlayer.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);

        List<PackAsset> initBang = GlobalVeriables.Instance.Player.Hand.FindAll(card => card.PackSprite.name.Contains("bang"));
        List<PackAsset> victimBang = victimPlayer.Hand.FindAll(card => card.PackSprite.name.Contains("bang"));

        _CreateCard().sprite = GlobalVeriables.Instance.Player.CharacterImage.sprite;

        foreach (PackAsset card in initBang)
            _CreateCard().sprite = card.PackSprite;

        _CreateCard().sprite = victimPlayer.CharacterImage.sprite;

        foreach (PackAsset card in victimBang)
            _CreateCard().sprite = card.PackSprite;

        if (initBang.Count < victimBang.Count)
        {
            GlobalVeriables.Instance.Player.Hit(victimPlayer);
            GlobalVeriables.Instance.Player.ShowBulletHole();
        }
        else
        {
            victimPlayer.Hit(GlobalVeriables.Instance.Player);
            victimPlayer.ShowBulletHole();
        }

        GlobalVeriables.Instance.audioSource.Play();
    }

    public static void Duel(Bot init, Character victim, PackAsset currentCard)
    {
        _Duel(init, victim, currentCard);
    }

    private static Image _CreateCard()
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Image cardImage = cardObject.AddComponent<Image>();
        return cardImage;
    }

    public static void Duel(Bot initPlayer, Bot victimPlayer, PackAsset currentCard)
    {
        initPlayer.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        
        List<PackAsset> initBang = initPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang"));
        List<PackAsset> victimBang = victimPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang"));

        if (initBang.Count < victimBang.Count)
        {
            initPlayer.Hit(initPlayer);
        }
        else
        {
            victimPlayer.Hit(initPlayer);
            victimPlayer.ShowBulletHole();
        }
    }
}
