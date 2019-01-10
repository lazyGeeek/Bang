using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/PanicAsset")]
public class PanicLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        UIElements.Instance.CardZone.ClearCardSpawn();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            Button enemyCard = Actions.CreateCard(enemy);
            enemyCard.onClick.AddListener(delegate { Panic(UIElements.Instance.Player, enemy, this); });
        }
    }

    public static bool Panic(Character init, Character victim, PackAsset currentCard)
    {
        List<PackAsset> victimCard = new List<PackAsset>();
        victimCard.AddRange(victim.Hand);
        victimCard.AddRange(victim.Buffs);
        if (victim.Weapon != null)
            victimCard.Add(victim.Weapon);

        if (victimCard.Count == 0)
            return false;

        if (init.Type == ECharacterType.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();

            GameObject emptyObject = new GameObject();
            foreach (PackAsset card in victimCard)
            {
                Button cardButton = Actions.CreateCard(card);
                cardButton.onClick.AddListener(delegate { GetCard(init, victim, card, currentCard); });
            }
            Destroy(emptyObject);
        }
        else
        {
            GetCard(init, victim, AIPickUpCard.PickUpCard(init, victimCard), currentCard);
        }

        return true;
    }

    public static void GetCard(Character init, Character victim, PackAsset dropCard, PackAsset currentCard)
    {
        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);

        if (victim.Buffs.Exists(asset => asset.PackSprite == dropCard.PackSprite))
        {
            victim.RemoveBuff(dropCard);
            init.AddCardToHand(dropCard);
        }
        else if (dropCard.CardType == ECardType.Weapon)
        {
            victim.Weapon = null;
            init.AddCardToHand(dropCard);
        }
        else
            init.AddCardToHand(dropCard);

        if (init.Type == ECharacterType.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();
            Actions.Instance.ShowPlayerCards();
        }
    }
}
