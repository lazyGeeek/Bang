using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BeautyAsset")]
public class BeautyLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            Button enemyCard = Actions.CreateCard(enemy);
            enemyCard.onClick.AddListener(delegate { _Beauty(enemy, this); });
        }
    }

    private void _Beauty(Bot enemy, PackAsset currentCard)
    {
        if (enemy.Hand.Count == 0 &&
            enemy.Buffs.Count == 0 &&
            enemy.Weapon == null)
        {
            GlobalVeriables.Instance.CardZone.ShowMessage("Enemy doesnt have any card");
            GlobalVeriables.Instance.CardZone.ClearCardSpawn();
            Actions.ShowPlayerCards();
        }
        else
        {
            GlobalVeriables.Instance.Player.Hand.Remove(currentCard);
            GlobalVeriables.Instance.Player.UsedCard.Add(currentCard);
            PackAndDiscard.Instance.Discard(currentCard);

            foreach (PackAsset card in enemy.Hand)
            {
                Image cardImage = _CreateCard(enemy, card);
                cardImage.sprite = enemy.BackRoleImage.sprite;
            }

            foreach (PackAsset buff in enemy.Buffs)
            {
                Image buffImage = _CreateCard(enemy, buff);
                buffImage.sprite = buff.PackSprite;
            }

            if (enemy.Weapon != null)
            {
                Image weaponImage = _CreateCard(enemy, enemy.Weapon);
                weaponImage.sprite = enemy.Weapon.PackSprite;
            }
        }
    }

    private Image _CreateCard(Bot enemy, PackAsset cardAsset)
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        cardButton.onClick.AddListener(delegate { _DropCard(enemy, cardAsset); });
        Image cardImage = cardObject.AddComponent<Image>();
        return cardImage;
    }

    private void _DropCard(Bot enemy, PackAsset currentCard)
    {
        PackAndDiscard.Instance.Discard(currentCard);

        switch (currentCard.CardType)
        {
            case ECardType.Buff:
                if (enemy.Buffs.Contains(currentCard))
                    enemy.RemoveBuff(currentCard);
                else
                    enemy.Hand.Remove(currentCard);
                break;
            case ECardType.Act:
                enemy.Hand.Remove(currentCard);
                break;
            case ECardType.Weapon:
                if (enemy.Weapon == currentCard)
                    enemy.Weapon = null;
                else
                    enemy.Hand.Remove(currentCard);
                break;
            default:
                Debug.Log("Unknown type");
                break;
        }

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        Actions.ShowPlayerCards();
    }

    public static bool Beauty(Bot init, Character victim, PackAsset currentCard)
    {
        List<PackAsset> victimCard = new List<PackAsset>();
        victimCard.AddRange(victim.Hand);
        victimCard.AddRange(victim.Buffs);

        if (victim.Weapon != null)
            victimCard.Add(victim.Weapon);

        if (victimCard.Count < 1)
            return false;

        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);

        //Not AIPickUpCard.PickUpCard(init, victimCard) because most of card is unknown 
        PackAsset cardToDrop = victimCard[Random.Range(0, victimCard.Count)];

        if (cardToDrop == victim.Weapon)
            victim.Weapon = null;
        else if (victim.Buffs.Contains(cardToDrop))
            victim.RemoveBuff(cardToDrop);
        else
            victim.Hand.Remove(cardToDrop);

        PackAndDiscard.Instance.Discard(cardToDrop);
        
        return true;
    }
}
