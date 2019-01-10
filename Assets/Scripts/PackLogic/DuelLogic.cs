using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/DuelAsset")]
public class DuelLogic : PackAsset
{
    [SerializeField]
    private AudioClip shootAC;

    public override void OnCardClick()
    {
        base.OnCardClick();

        UIElements.Instance.CardZone.ClearCardSpawn();
        UIElements.Instance.audioSource.clip = shootAC;

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            Button enemyCard = Actions.CreateCard(enemy);
            enemyCard.onClick.AddListener(delegate { Duel(UIElements.Instance.Player, enemy, this); });
        }
    }

    public static void Duel(Character initPlayer, Character victimPlayer, PackAsset currentCard)
    {
        UIElements.Instance.CardZone.ShowCardSpawn();
        UIElements.Instance.CardZone.ClearCardSpawn();
        UIElements.Instance.CardZone.dropCardButton.gameObject.SetActive(false);
        initPlayer.RemoveCardToDiscard(currentCard);
        initPlayer.UsedCard.Add(currentCard);

        List<PackAsset> initBang = initPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang"));
        List<PackAsset> victimBang = victimPlayer.Hand.FindAll(l => l.PackSprite.name.Contains("bang"));

        GameObject emptyObject = new GameObject();
        GameObject initObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Image initImage = initObject.AddComponent<Image>();
        initImage.sprite = initPlayer.CharacterImage.sprite;

        foreach (PackAsset card in initBang)
        {
            GameObject initCardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
            Image initCard = initCardObject.AddComponent<Image>();
            initCard.sprite = card.PackSprite;
        }

        GameObject victimObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Image victimImage = victimObject.AddComponent<Image>();
        victimImage.sprite = victimPlayer.CharacterImage.sprite;

        foreach (PackAsset card in victimBang)
        {
            GameObject victimCardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
            Image victimCard = victimCardObject.AddComponent<Image>();
            victimCard.sprite = card.PackSprite;
        }

        Destroy(emptyObject);

        if (initBang.Count < victimBang.Count)
        {
            initPlayer.Hit(initPlayer);
            initPlayer.ShowBulletHole();
        }
        else
        {
            victimPlayer.Hit(initPlayer);
            victimPlayer.ShowBulletHole();
        }

        UIElements.Instance.audioSource.Play();
    }
}
