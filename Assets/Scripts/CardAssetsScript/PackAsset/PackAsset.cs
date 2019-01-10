using UnityEngine;
using UnityEngine.UI;

public class PackAsset : ScriptableObject
{
    public Sprite PackSprite;
    public ECardType CardType;
    public ECardName CardName;
    public ECardRating CardRating;
    public ECardSuit CardSuit;
    public Button CurrentCard { get; set; }

    public virtual void OnCardClick()
    {
        if (UIElements.Instance.Player.UsedCard.Exists(card => card.CardName == CardName))
        {
            UIElements.Instance.CardZone.ShowMessage("You already use this card");
            return;
        }
    }

    public static bool operator ==(PackAsset p1, PackAsset p2)
    {
        return ReferenceEquals(p1, p2);
    }

    public static bool operator !=(PackAsset p1, PackAsset p2)
    {
        return !(p1 == p2);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        if (other == null) return false;
        return this == (PackAsset)other;
    }
}
