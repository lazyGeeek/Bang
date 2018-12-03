using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BangAsset")]
public class BangLogic : PackAsset
{
    [SerializeField]
    private AudioClip shootAC;

    public override void OnCardClick()
    {
        if (UIElements.Instance.Player.UsedCard.Exists(card => card.CardName == ECardName.Bang) &&
            !UIElements.Instance.Player.Buffs.Exists(card => card.CardName == ECardName.Rage))
        {
            UIElements.Instance.CardZone.ShowMessage("You already use this card");
            return;
        }

        List<Character> ScopeEnemies = new List<Character>(Actions.GetScopeEnemies());

        if (ScopeEnemies.Count == 0)
        {
            UIElements.Instance.CardZone.ShowMessage("You can't see anyone!");
            return;
        }

        UIElements.Instance.audioSource.clip = shootAC;

        if (UIElements.Instance.Player.UsedCard.Exists(card => card.CardName == ECardName.Bang) &&
            UIElements.Instance.Player.Buffs.Exists(card => card.CardName == ECardName.Rage))
        {
            foreach (Character enemy in ScopeEnemies)
            {
                if (enemy.Position != 1)
                    ScopeEnemies.Remove(enemy);
            }
        }
        
        if (ScopeEnemies.Count == 0)
        {
            UIElements.Instance.CardZone.ShowMessage("You can't see anyone!");
            return;
        }
        else
        {
            UIElements.Instance.CardZone.ClearCardSpawn();

            foreach (Character enemy in ScopeEnemies)
            {
                Button enemyButton = Actions.CreateCard(enemy);
                enemyButton.onClick.AddListener(delegate { Bang(UIElements.Instance.Player, enemy, this); });
            }
        }
    }

    public static void Bang(Character killer, Character victim, PackAsset currentCard)
    {
        killer.RemoveCardToDiscard(currentCard);
        killer.UsedCard.Add(currentCard);

        UIElements.Instance.audioSource.Play();

        if (victim.Type == ECharacterType.Player)
        {
            List<PackAsset> defenseCard = new List<PackAsset>();
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Missed));
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Beer));
            defenseCard.AddRange(UIElements.Instance.Player.Buffs.FindAll(card => card.CardName == ECardName.Barrel));

            if (defenseCard.Count < 1)
            {
                victim.Hit();
                victim.ShowBulletHole();
            }
            else
            {
                UIElements.Instance.CardZone.ShowCardSpawn();
                UIElements.Instance.CardZone.ClearCardSpawn();
                GlobalVeriables.GameState = EGameState.Defense;

                foreach (PackAsset card in defenseCard)
                    Actions.CreateCard(card);
            }
        }
        else
        {
            AIDefense.Defense(victim, killer);

            if (killer.Type == ECharacterType.Player)
            {
                UIElements.Instance.CardZone.ClearCardSpawn();
                Actions.Instance.ShowPlayerCards();
            }
        }
    }
}
