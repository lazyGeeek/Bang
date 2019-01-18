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

        _Saloon(GlobalVeriables.Instance.Player, this);
        Destroy(CurrentCard.gameObject);
    }

    private static void _Saloon(Player init, PackAsset currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        init.UsedCard.Add(currentCard);

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (!enemy.IsDead)
                enemy.Heal();
        }
    }

    public static void Saloon(Bot init, PackAsset currentCard)
    {
        init.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);

        if (!GlobalVeriables.Instance.Player.IsDead)
            GlobalVeriables.Instance.Player.Heal();

        foreach (Bot enemy in GlobalVeriables.Instance.Enemies)
        {
            if (!enemy.IsDead)
                enemy.Heal();
        }
    }
}
