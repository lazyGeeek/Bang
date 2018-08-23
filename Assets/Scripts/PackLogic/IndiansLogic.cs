using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndiansLogic : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Button defenseCard;

    public static IndiansLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartIndians(Character init)
    {
        StartCoroutine(IndiansCorountine(init));
    }

    private IEnumerator IndiansCorountine(Character init)
    {
        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy != init)
            {
                PackAsset bangCard = enemy.Hand.Exists(asset => asset.CardName == ECardName.Bang) ?
                    enemy.Hand.Find(asset => asset.CardName == ECardName.Bang) : null;

                if (bangCard != null)
                    enemy.RemoveCardFromHand(bangCard);
                else
                    enemy.Hit();
            }
        }

        audioSource.Play();

        if (UIElements.Instance.Player != init)
        {
            GlobalVeriable.GameState = EGameState.Defense;
            ShowCards.Instance.ShowCardSpawn();
            ShowCards.Instance.ClearCardSpawn();

            if (UIElements.Instance.Player.Hand.Exists(card => card.CardName == ECardName.Bang))
            {
                foreach (PackAsset bang in UIElements.Instance.Player.Hand.FindAll(bang => bang.CardName == ECardName.Bang))
                {
                    Button card = ShowCards.Instance.cardSpawn.AddComponent<Button>();
                    card.image.sprite = bang.PackSprite;
                    card.GetComponent<Pack>().CurrentCard = bang;
                }
            }
        }
        else
            yield return new WaitForSeconds(6f);
    }
}
