using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/DynamiteAsset")]
public class DynamiteLogic : PackAsset
{
    [SerializeField]
    private GameObject explosionAnimation;

    [SerializeField]
    private AudioClip explosioAC;

    private static DynamiteLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnCardClick()
    {
        GlobalVeriable.IsDynamite = true;
        GlobalVeriable.DynamiteInit = UIElements.Instance.Player;
        UIElements.Instance.Player.RemoveCardToDiscard(this);
        Destroy(CurrentCard.gameObject);
        UIElements.Instance.CardZone.ShowMessage("You throw dynamite");
    }

    public static void Dynamite(int pos)
    {
        UIElements.Instance.CardZone.ShowCardSpawn();

        Character[] AllCharacters =
        {
            UIElements.Instance.Player,
            UIElements.Instance.Enemies[0],
            UIElements.Instance.Enemies[1],
            UIElements.Instance.Enemies[2],
            UIElements.Instance.Enemies[3],
        };

        GameObject emptyObject = new GameObject();

        GameObject playerObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Image player = playerObject.AddComponent<Image>();
        AudioSource explosion = playerObject.AddComponent<AudioSource>();
        explosion.clip = Instance.explosioAC;

        GameObject cardObject = Instantiate(emptyObject, UIElements.Instance.CardZone.cardSpawn.transform);
        Image card = cardObject.AddComponent<Image>();

        Destroy(emptyObject);

        while (true)
        {
            Character character = AllCharacters[pos++];
            player.sprite = character.CharacterImage.sprite;
            PackAsset cardAsset = PackAndDiscard.Instance.GetRandomCard();
            card.sprite = cardAsset.PackSprite;

            if (cardAsset.CardSuit == ECardSuit.Spades && (int)cardAsset.CardRating >= 2 && (int)cardAsset.CardRating < 10)
            {
                explosion.Play();

                for (int j = 0; j < 3; ++j)
                {
                    if (!character.IsDead)
                        character.Hit();
                }
                
                PackAndDiscard.Instance.Discard(cardAsset);
                break;
            }

            if (pos == AllCharacters.Length)
                pos = 0;
            
            PackAndDiscard.Instance.Discard(cardAsset);

            Actions.Wait(2f);
        }
    }
}
