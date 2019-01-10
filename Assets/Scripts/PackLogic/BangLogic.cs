using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Assets/BangAsset")]
public class BangLogic : PackAsset
{
    [SerializeField]
    private AudioClip shootAC;

    public override void OnCardClick() //TODO refactoring
    {
        if (UIElements.Instance.Player.UsedCard.Exists(card => card.CardName == ECardName.Bang) &&
            !UIElements.Instance.Player.Buffs.Exists(card => card.CardName == ECardName.Rage))
        {
            UIElements.Instance.CardZone.ShowMessage("You already use this card");
            return;
        }

        List<Character> ScopeEnemies = new List<Character>(Actions.GetScopeEnemies(UIElements.Instance.Player));

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

    public static void Bang(Character killer, Character victim, BangLogic currentCard)
    {
        killer.Hand.Remove(currentCard);
        killer.UsedCard.Add(currentCard);

        UIElements.Instance.audioSource.clip = currentCard.shootAC;
        UIElements.Instance.audioSource.Play();
        
        if (victim.Type == ECharacterType.Player)
        {
            List<PackAsset> defenseCard = new List<PackAsset>();
            defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Missed));

            PackAsset barrel = UIElements.Instance.Player.Buffs.Find(card => card.CardName == ECardName.Barrel);
            if (barrel != null)
                defenseCard.Add(barrel);
            
            if (victim.CurrentHealth == 1)
                defenseCard.AddRange(UIElements.Instance.Player.Hand.FindAll(card => card.CardName == ECardName.Beer));

            if (defenseCard.Count == 0)
            {
                UIElements.Instance.Player.Hit();
            }
            else
            {
                UIElements.Instance.CardZone.ShowCardSpawn();
                UIElements.Instance.CardZone.ClearCardSpawn();
                UIElements.Instance.CardZone.dropCardButton.gameObject.SetActive(false); 
                GlobalVeriables.GameState = EGameState.Defense;

                foreach (PackAsset card in defenseCard)
                {
                    Actions.CreateCard(card);
                }
            }
        }
        else
        {
            if (!victim.botEnemies.Contains(killer))
                victim.botEnemies.Add(killer);

            AIDefense.Defense(victim, killer);

            if (killer.Type == ECharacterType.Player)
            {
                UIElements.Instance.CardZone.ClearCardSpawn();
                Actions.Instance.ShowPlayerCards();
            }
        }
    }
}
