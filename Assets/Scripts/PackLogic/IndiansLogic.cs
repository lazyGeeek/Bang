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
        UIElements.Instance.audioSource.clip = currentCard.audioSource;
        UIElements.Instance.audioSource.Play();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy != init)
            {
                PackAsset bangCard = enemy.Hand.Find(asset => asset.CardName == ECardName.Bang);

                if (bangCard != null)
                    enemy.RemoveCardFromHand(bangCard);
                else
                    enemy.Hit();
            }
        }

        if (UIElements.Instance.Player != init && !UIElements.Instance.Player.IsDead)
        {
            List<PackAsset> bangs = new List<PackAsset>(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Bang));

            if (bangs.Count == 0)
            {
                UIElements.Instance.Player.Hit();
            }
            else
            {
                GlobalVeriables.GameState = EGameState.Defense;
                UIElements.Instance.CardZone.ShowCardSpawn();
                UIElements.Instance.CardZone.ClearCardSpawn();
                UIElements.Instance.CardZone.dropCardButton.gameObject.SetActive(false);
                UIElements.Instance.CardZone.ShowPermanentMessage("Drop bang or take hit");

                GameObject emptyObject = new GameObject();

                foreach (PackAsset bang in bangs)
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
        GlobalVeriables.GameState = EGameState.Move;
        UIElements.Instance.Player.Hand.Remove(bangCard);

        if (GlobalVeriables.CurrentPlayer == UIElements.Instance.Player)
        {
            UIElements.Instance.CardZone.ClearCardSpawn();
            Actions.Instance.ShowPlayerCards();
        }
        else
            UIElements.Instance.CardZone.Close();
    }
}
