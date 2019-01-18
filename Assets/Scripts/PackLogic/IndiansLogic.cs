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

        GlobalVeriables.Instance.Player.StartCoroutine(_Indians(GlobalVeriables.Instance.Player, this));
        Destroy(CurrentCard);
    }

    private static IEnumerator _Indians(Player init, IndiansLogic currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        init.UsedCard.Add(currentCard);
        GlobalVeriables.Instance.audioSource.clip = currentCard.audioSource;
        GlobalVeriables.Instance.audioSource.Play();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            PackAsset bangCard = enemy.Hand.Find(asset => asset.CardName == ECardName.Bang);

            if (bangCard != null)
            {
                enemy.Hand.Remove(bangCard);
                PackAndDiscard.Instance.Discard(bangCard);
            }
            else
                enemy.Hit();
        }

        yield return new WaitForSeconds(GlobalVeriables.Instance.audioSource.time);
    }

    public static IEnumerator Indians(Bot init, IndiansLogic currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        GlobalVeriables.Instance.audioSource.clip = currentCard.audioSource;
        GlobalVeriables.Instance.audioSource.Play();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (enemy != init)
            {
                PackAsset bangCard = enemy.Hand.Find(asset => asset.CardName == ECardName.Bang);

                if (bangCard != null)
                {
                    enemy.Hand.Remove(bangCard);
                    PackAndDiscard.Instance.Discard(bangCard);
                }
                else
                    enemy.Hit();
            }
        }

        if (!GlobalVeriables.Instance.Player.IsDead)
        {
            List<PackAsset> bangs = new List<PackAsset>(GlobalVeriables.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Bang));

            if (bangs.Count == 0)
            {
                GlobalVeriables.Instance.Player.Hit();
            }
            else
            {
                GlobalVeriables.GameState = EGameState.Defense;
                GlobalVeriables.Instance.CardZone.ShowCardSpawn(true, false);
                GlobalVeriables.Instance.CardZone.ShowPermanentMessage("Drop bang or take hit");

                foreach (PackAsset bang in bangs)
                    _CreateCard(bang);
            }
            
        }
        else
            yield return new WaitForSeconds(GlobalVeriables.Instance.audioSource.time);
    }

    private static void _CreateCard(PackAsset card)
    {
        GameObject cardObject = new GameObject();
        cardObject.transform.SetParent(GlobalVeriables.Instance.CardZone.cardSpawn.transform, false);
        Button cardButton = cardObject.AddComponent<Button>();
        Image cardImage = cardObject.AddComponent<Image>();
        cardImage.sprite = card.PackSprite;
        cardButton.onClick.AddListener(delegate { _DropBangCard(card); });
    }

    private static void _DropBangCard(PackAsset bangCard)
    {
        GlobalVeriables.GameState = EGameState.Move;
        GlobalVeriables.Instance.Player.Hand.Remove(bangCard);
        PackAndDiscard.Instance.Discard(bangCard);
        GlobalVeriables.Instance.CardZone.Close();
    }
}
