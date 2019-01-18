using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
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
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        cardButton.onClick.AddListener(cardAsset.OnCardClick);
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = cardAsset.PackSprite;
        cardAsset.CurrentCard = cardObject;
        return cardButton;
    }

    public static Button CreateCard(Character ch)
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = ch.CharacterImage.sprite;
        return cardButton;
    }

    public static List<Character> GetScopeEnemies(Character character, bool closeRange)
    {
        List<Character> enemies = new List<Character>();

        foreach (KeyValuePair<Character, int> scope in character.EnemyVisibility)
        {
            int mustang = scope.Key.Buffs.Find(card => card.CardName == ECardName.Mustang) == null ? 0 : 1;
            int appaloosa = character.Buffs.Find(card => card.CardName == ECardName.Appaloosa) == null ? 0 : 1;

            if (!closeRange)
            {
                if ((!scope.Key.IsDead) && ((character.Scope + appaloosa) >= (scope.Value + mustang)))
                    enemies.Add(scope.Key);
            }
            else
            {
                if ((!scope.Key.IsDead) && ((1 + appaloosa) >= (scope.Value + mustang)))
                    enemies.Add(scope.Key);
            }
        }

        return enemies;
    }

    public static void ShowPlayerCards()
    {
        foreach (PackAsset card in GlobalVeriables.Instance.Player.Hand)
        {
            CreateCard(card);
        }
    }

    public void PlayerCards()
    {
        ShowPlayerCards();
    }
}
