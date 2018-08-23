using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamiteLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private Image dynamiteCard;

    [SerializeField]
    private AudioSource audioS;

    public static DynamiteLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator DynamiteCoroutine(int pos)
    {
        ShowCards.Instance.ShowCardSpawn();

        Character[] AllCharacters =
        {
            UIElements.Instance.Player,
            UIElements.Instance.Enemies[0],
            UIElements.Instance.Enemies[1],
            UIElements.Instance.Enemies[2],
            UIElements.Instance.Enemies[3],
        };

        Image player = Instantiate(dynamiteCard, ShowCards.Instance.cardSpawn.transform);
        Image card = Instantiate(dynamiteCard, ShowCards.Instance.cardSpawn.transform);

        while (true)
        {
            Character character = AllCharacters[pos++];
            player.sprite = character.CharacterInfo.CharacterSprite;
            PackAsset cardAsset = PackAndDiscard.Instance.GetRandomCard();
            card.sprite = cardAsset.PackSprite;

            if (cardAsset.CardSuit == ECardSuit.Spades && (int)cardAsset.CardRating >= 2 && (int)cardAsset.CardRating < 10)
            {
                Instantiate(audioS, player.transform);
                Instantiate(explosion, player.transform);

                for (int j = 0; j < 3; ++j)
                {
                    if (!character.IsDead)
                        character.Hit();
                }
                
                PackAndDiscard.Instance.Discard(cardAsset);
                yield break;
            }

            if (pos == AllCharacters.Length)
                pos = 0;
            
            PackAndDiscard.Instance.Discard(cardAsset);

            yield return new WaitForSeconds(2f);
        }

        /*while (true)
        {
            player.sprite = UIElements.Instance.Enemies[i].CharacterInfo.CharacterSprite;
            PackAsset cardAsset = PackAndDiscard.Instance.GetRandomCard();
            card.sprite = cardAsset.PackSprite;

            if (reg.IsMatch(card.sprite.name))
            {
                for (int j = 0; j < 3; ++j)
                    UIElements.Instance.Enemies[i].Hit();

                yield return new WaitForSeconds(2f);
                PackAndDiscard.Instance.Discard(cardAsset);
                UIElements.Instance.ShowCards.Close();
                yield break;
            }

            do
            {
                if (++i == UIElements.Instance.Enemies.Length)
                {
                    i = 0;
                    if (!UIElements.Instance.Player.IsDead)
                    {
                        player.sprite = UIElements.Instance.Player.CharacterInfo.CharacterSprite;

                        cardAsset = PackAndDiscard.Instance.GetRandomCard();
                        card.sprite = cardAsset.PackSprite;

                        if (reg.IsMatch(card.sprite.name))
                        {
                            for (int j = 0; j < 3; ++j)
                                UIElements.Instance.Player.Hit();

                            yield return new WaitForSeconds(2f);
                            PackAndDiscard.Instance.Discard(cardAsset);
                            UIElements.Instance.ShowCards.Close();
                            yield break;
                        }
                    }
                }
            } while (UIElements.Instance.Enemies[i].IsDead);

            PackAndDiscard.Instance.Discard(cardAsset);

            yield return new WaitForSeconds(2f);
        }*/
    }
}
