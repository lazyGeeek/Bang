using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public static Actions Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void Wait(float seconds)
    {
        Instance.StartCoroutine(WaitCoroutine(seconds));
    }

    private static IEnumerator WaitCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public static int GetScope(PackAsset s)
    {
        if (s == null)
            return 1;

        if (s.CardName == ECardName.Colt) return 2;
        else if (s.CardName == ECardName.Remington) return 3;
        else if (s.CardName == ECardName.Carabine) return 4;
        else if (s.CardName == ECardName.Volcano) return 5;
        return 1;
    }

    public static Button CreateCard(PackAsset cardAsset)
    {
        GameObject emptyObject = new GameObject();
        GameObject cardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Button card = cardObject.AddComponent<Button>();
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = cardAsset.PackSprite;
        Destroy(emptyObject);
        return card;
    }

    public static Button CreateCard(Character ch)
    {
        GameObject emptyObject = new GameObject();
        GameObject cardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Button card = cardObject.AddComponent<Button>();
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = ch.CharacterImage.sprite;
        Destroy(emptyObject);
        return card;
    }

    public static IEnumerable<Character> GetScopeEnemies()
    {
        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (!enemy.IsDead && UIElements.Instance.Player.Scope >= enemy.Position)
                yield return enemy;
        }
    }

    public void ShowPlayerCards()
    {
        UIElements.Instance.CardZone.ShowCardSpawn();

        GameObject emptyObject = new GameObject();
        foreach (PackAsset card in UIElements.Instance.Player.Hand)
        {
            GameObject cardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
            Button cardButton = cardObject.AddComponent<Button>();
            cardButton.onClick.AddListener(card.OnCardClick);
            Image cardImage = cardObject.AddComponent<Image>();
            cardImage.sprite = card.PackSprite;
            card.CurrentCard = cardButton;
        }
        Destroy(emptyObject);
    }
}
