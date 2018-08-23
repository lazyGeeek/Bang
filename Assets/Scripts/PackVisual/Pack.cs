using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour
{
    public PackAsset CurrentCard { get; set; }

    public void GetBuff()
    {
        if (!UIElements.Instance.Player.Buffs.Contains(CurrentCard))
        {
            UIElements.Instance.Player.AddBuff(CurrentCard);
            Destroy(this);
        }
        else
            ShowCards.Instance.ShowMessage("You already have this buff");
    }

    public void Defense()
    {
        ShowCards.Instance.Close();
        GlobalVeriable.GameState = EGameState.Move;
        UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);
        PackAndDiscard.Instance.Discard(CurrentCard);
    }

    public void ShowScopeEnemies()
    {
        //If card is used method do nothing else add card in list of used card
        if (UIElements.Instance.Player.UsedCard.Find(card => card.CardName == CurrentCard.CardName))
        {
            ShowCards.Instance.ShowMessage("You already use this card");
            return;
        }

        ShowCards.Instance.ClearCardSpawn();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (!enemy.IsDead && UIElements.Instance.Player.Scope >= enemy.Position)
            {
                Button player = Instantiate(CurrentCard.secondStageButton, ShowCards.Instance.cardSpawn.transform);
                player.image.sprite = enemy.CharacterInfo.CharacterSprite;
                GlobalVeriable.CurrentEnemy = enemy;
                GlobalVeriable.CurrentCard = CurrentCard;
            }
        }

        if (ShowCards.Instance.cardSpawn.GetComponentsInChildren<Button>().Length < 1)
        {
            ShowCards.Instance.ShowMessage("You can't see anyone");
            Actions.Instance.ShowPlayerCards();
        }
    }

    public void ShowAllEnemies()
    {
        //If card is used method do nothing else add card in list of used card
        if (UIElements.Instance.Player.UsedCard.Find(card => card.CardName == CurrentCard.CardName))
        {
            ShowCards.Instance.ShowMessage("You already use this card");
            return;
        }

        //UIElements.Instance.Player.RemoveCard(CurrentCard);
        ShowCards.Instance.ClearCardSpawn();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            Button player = Instantiate(CurrentCard.secondStageButton, ShowCards.Instance.cardSpawn.transform);
            player.image.sprite = enemy.CharacterInfo.CharacterSprite;
            GlobalVeriable.CurrentEnemy = enemy;
            GlobalVeriable.CurrentCard = CurrentCard;
        }
    }

    public void DrinkBeer()
    {
        if (UIElements.Instance.Player.Heal())
        {
            UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);
            PackAndDiscard.Instance.Discard(CurrentCard);
            Destroy(this);
        }
        else
            ShowCards.Instance.ShowMessage("You have full health");
    }

    public void ChangePlayerGun()
    {
        UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);

        if (UIElements.Instance.Player.Weapon != null)
        {
            UIElements.Instance.Player.AddCardToHand(UIElements.Instance.Player.Weapon);
            UIElements.Instance.Player.Weapon = CurrentCard;
        }
        else
        {
            UIElements.Instance.Player.Weapon = CurrentCard;
            Destroy(gameObject);
        }
        
        UIElements.Instance.Player.Scope = Actions.GetScope(UIElements.Instance.Player.Weapon);
    }

    public void GetPackCards(int quantity)
    {
        for (int i = 0; i < quantity; ++i)
        {
            UIElements.Instance.Player.AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
        }

        UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);
        PackAndDiscard.Instance.Discard(CurrentCard);
        Destroy(gameObject);
    }

    public void ThrowDynamite()
    {
        UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);
        PackAndDiscard.Instance.Discard(CurrentCard);

        foreach (Button card in ShowCards.Instance.cardSpawn.GetComponentsInChildren<Button>())
        {
            if (card.image.sprite != CurrentCard.PackSprite)
                Destroy(card);
        }

        StartCoroutine(DynamiteLogic.Instance.DynamiteCoroutine(0));
    }

    public void GetGatling()
    {
        UIElements.Instance.Player.RemoveCardFromHand(CurrentCard);
        PackAndDiscard.Instance.Discard(CurrentCard);

        GatlingLogic.Instance.StartGatling(UIElements.Instance.Player);

        Destroy(gameObject);
    }

    public void GetIndians()
    {
        UIElements.Instance.Player.RemoveCardToDiscard(CurrentCard);
        IndiansLogic.Instance.StartIndians(UIElements.Instance.Player);
    }

    public void GetSaloon()
    {
        UIElements.Instance.Player.RemoveCardToDiscard(CurrentCard);
        SaloonLogic.Saloon(UIElements.Instance.Player);
    }

    public void StartStore()
    {
        UIElements.Instance.Player.RemoveCardToDiscard(CurrentCard);
        StoreLogic.Store(UIElements.Instance.Player);
    }

    public void GetStoreCard()
    {
        UIElements.Instance.Player.AddCardToHand(CurrentCard);
        StoreLogic.ContinueStore(CurrentCard);
    }
}
