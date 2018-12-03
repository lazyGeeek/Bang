using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/IndiansAsset")]
public class IndiansLogic : PackAsset
{
    public AudioClip audioSource;

    public override void OnCardClick()
    {
        base.OnCardClick();

        Indians(UIElements.Instance.Player, this);
        Destroy(CurrentCard);
    }

    public static void Indians(Character init, IndiansLogic currentCard)
    {
        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy.CharacterImage.sprite != init.CharacterImage.sprite)
            {
                PackAsset bangCard = enemy.Hand.Exists(asset => asset.CardName == ECardName.Bang) ?
                    enemy.Hand.Find(asset => asset.CardName == ECardName.Bang) : null;

                if (bangCard != null)
                    enemy.RemoveCardFromHand(bangCard);
                else
                    enemy.Hit();
            }
        }

        UIElements.Instance.audioSource.clip = currentCard.audioSource;
        UIElements.Instance.audioSource.Play();

        if (UIElements.Instance.Player.CharacterImage.sprite != init.CharacterImage.sprite)
        {
            GlobalVeriables.GameState = EGameState.Defense;
            UIElements.Instance.CardZone.ShowCardSpawn();
            UIElements.Instance.CardZone.ClearCardSpawn();

            if (UIElements.Instance.Player.Hand.Exists(card => card.CardName == ECardName.Bang))
            {
                GameObject emptyObject = new GameObject();
                foreach (PackAsset bang in UIElements.Instance.Player.Hand.FindAll(bang => bang.CardName == ECardName.Bang))
                {
                    GameObject cardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
                    Button card = cardObject.AddComponent<Button>();
                    Image cardImage = cardObject.AddComponent<Image>();
                    cardImage.sprite = bang.PackSprite;
                    card.onClick.AddListener(delegate { DropBangCard(bang); });
                }
                Destroy(emptyObject);
            }
        }
        else
            Actions.Wait(UIElements.Instance.audioSource.time);
    }

    private static void DropBangCard(PackAsset bangCard)
    {
        UIElements.Instance.Player.RemoveCardToDiscard(bangCard);
        UIElements.Instance.CardZone.ClearCardSpawn();
        Actions.Instance.ShowPlayerCards();
    }
}
