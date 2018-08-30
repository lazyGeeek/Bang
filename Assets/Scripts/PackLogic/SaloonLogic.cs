using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/SaloonAsset")]
public class SaloonLogic : PackAsset
{
    public override void OnCardClick()
    {
        base.OnCardClick();

        Saloon(UIElements.Instance.Player, this);
        Destroy(CurrentCard.gameObject);
    }

    public static void Saloon(Character init, PackAsset currentCard)
    {
        init.RemoveCardToDiscard(currentCard);
        init.UsedCard.Add(currentCard);

        if (!UIElements.Instance.Player.IsDead)
            UIElements.Instance.Player.Heal();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (!enemy.IsDead)
                UIElements.Instance.Player.Heal();
        }
    }
}
