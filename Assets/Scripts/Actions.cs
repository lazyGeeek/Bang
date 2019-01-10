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
        if (s == null) return 1;

        if (s.CardName == ECardName.Colt) return 2;
        else if (s.CardName == ECardName.Remington) return 3;
        else if (s.CardName == ECardName.Carabine) return 4;
        else if (s.CardName == ECardName.Volcano) return 5;
        return 1;
    }

    public static Button CreateCard(PackAsset cardAsset)
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(UIElements.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        cardButton.onClick.AddListener(cardAsset.OnCardClick);
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = cardAsset.PackSprite;
        cardAsset.CurrentCard = cardButton;
        return cardButton;
    }

    public static Button CreateCard(Character ch)
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(UIElements.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = ch.CharacterImage.sprite;
        return cardButton;
    }

    public static List<Character> GetScopeEnemies(Character character, bool closeRange = false)
    {
        List<Character> enemies = new List<Character>();

        foreach (KeyValuePair<Character, int> scope in character.EnemyVisibility)
        {
            int mustang = scope.Key.Buffs.Find(card => card.CardName == ECardName.Mustang) == null ? 0 : 1;
            int appaloosa = character.Buffs.Find(card => card.CardName == ECardName.Appaloosa) == null ? 0 : 1;

            if (!scope.Key.IsDead && character.Scope + appaloosa >= scope.Value + mustang)
            {
                if (!closeRange || (closeRange && scope.Value + mustang <= 1))
                    enemies.Add(scope.Key);
            }
        }

        return enemies;
    }

    public void ShowPlayerCards() //TODO Check this
    {
        UIElements.Instance.CardZone.ShowCardSpawn();

        foreach (PackAsset card in UIElements.Instance.Player.Hand)
        {
            GameObject cardObject = new GameObject();
            cardObject.transform.SetParent(UIElements.Instance.CardZone.cardSpawn.transform, false);
            Button cardButton = cardObject.AddComponent<Button>();
            cardButton.onClick.AddListener(card.OnCardClick);
            Image cardImage = cardObject.AddComponent<Image>();
            cardImage.sprite = card.PackSprite;
            card.CurrentCard = cardButton;
        }

        if (GlobalVeriables.GameState == EGameState.Defense)
        {
            foreach (PackAsset card in UIElements.Instance.Player.Buffs)
            {
                GameObject cardObject = new GameObject();
                cardObject.transform.SetParent(UIElements.Instance.CardZone.cardSpawn.transform, false);
                Button cardButton = cardObject.AddComponent<Button>();
                cardButton.onClick.AddListener(card.OnCardClick);
                Image buffImage = cardObject.AddComponent<Image>();
                buffImage.sprite = card.PackSprite;
                card.CurrentCard = cardButton;
            }

            /*GameObject weaponObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
            Button weaponButton = weaponObject.AddComponent<Button>();
            weaponButton.onClick.AddListener(UIElements.Instance.Player.Weapon.OnCardClick);
            Image cardImage = weaponObject.AddComponent<Image>();
            cardImage.sprite = UIElements.Instance.Player.Weapon.PackSprite;
            UIElements.Instance.Player.Weapon.CurrentCard = weaponButton;*/
        }
    }
}
