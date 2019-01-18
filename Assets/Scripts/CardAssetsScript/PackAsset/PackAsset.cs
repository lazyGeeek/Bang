using UnityEngine;
using UnityEngine.UI;

public class PackAsset : ScriptableObject
{
    public Sprite PackSprite;
    public ECardType CardType;
    public ECardName CardName;
    public ECardRating CardRating;
    public ECardSuit CardSuit;
    public GameObject CurrentCard { get; set; }

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

    public virtual void OnCardClick()
    {
        if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            GlobalVeriables.Instance.Player.Hand.Remove(this);
            PackAndDiscard.Instance.Discard(this);
            Destroy(CurrentCard.gameObject);
            return;
        }
    }
}
