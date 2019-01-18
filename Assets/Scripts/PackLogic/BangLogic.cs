using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BangAsset")]
public class BangLogic : PackAsset
{
    [SerializeField]
    private readonly AudioClip shootAC;

    public override void OnCardClick()
    {
        /*if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            GlobalVeriables.Instance.Player.Hand.Remove(this);
            PackAndDiscard.Instance.Discard(this);
            Destroy(CurrentCard.gameObject);
            return;
        }*/
        base.OnCardClick();

        List<Character> scopeEnemies = new List<Character>();
        PackAsset usedBang = GlobalVeriables.Instance.Player.UsedCard.Find(card => card.CardName == ECardName.Bang);
        PackAsset rage = GlobalVeriables.Instance.Player.Buffs.Find(card => card.CardName == ECardName.Rage);

        if (usedBang != null && rage == null)
        {
            GlobalVeriables.Instance.CardZone.ShowMessage("You already use this card");
            return;
        }

        if (usedBang == null)
            scopeEnemies.AddRange(Actions.GetScopeEnemies(GlobalVeriables.Instance.Player, false));
        else if (usedBang != null && rage != null)
            scopeEnemies.AddRange(Actions.GetScopeEnemies(GlobalVeriables.Instance.Player, true));

        if (scopeEnemies.Count == 0)
        {
            GlobalVeriables.Instance.CardZone.ShowMessage("You can't see anyone!");
            return;
        }

        GlobalVeriables.Instance.audioSource.clip = shootAC;

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();

        foreach (Character enemy in scopeEnemies)
        {
            Button enemyButton = Actions.CreateCard(enemy);
            enemyButton.onClick.AddListener(delegate { _Bang((Bot)enemy, this); });
        }
    }

    private static void _Bang(Bot victim, PackAsset currentCard)
    {
        GlobalVeriables.Instance.Player.Hand.Remove(currentCard);
        GlobalVeriables.Instance.Player.UsedCard.Add(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        AIDefense.Defense(victim, GlobalVeriables.Instance.Player);

        GlobalVeriables.Instance.audioSource.Play();

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();
        Actions.ShowPlayerCards();
    }

    public static void Bang(Player victim, Bot enemy, PackAsset currentCard)
    {
        enemy.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        GlobalVeriables.GameState = EGameState.Defense;

        if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
            GlobalVeriables.Instance.CardZone.dropCardButton.gameObject.SetActive(false);
        else
            GlobalVeriables.Instance.CardZone.ShowCardSpawn(true, false);

        GlobalVeriables.Instance.CardZone.ClearCardSpawn();

        List<PackAsset> defenseCard = new List<PackAsset>();

        defenseCard.AddRange(victim.Hand.FindAll(card => card.CardName == ECardName.Missed));
        defenseCard.AddRange(victim.Buffs.FindAll(card => card.CardName == ECardName.Barrel));

        if (victim.CurrentHealth == 1)
            defenseCard.AddRange(victim.Hand.FindAll(card => card.CardName == ECardName.Beer));

        foreach (PackAsset card in defenseCard)
            Actions.CreateCard(card);
    }

    public static void Bang(Bot victim, Bot enemy, PackAsset currentCard)
    {
        enemy.Hand.Remove(currentCard);
        PackAndDiscard.Instance.Discard(currentCard);
        AIDefense.Defense(victim, enemy);
    }
}
