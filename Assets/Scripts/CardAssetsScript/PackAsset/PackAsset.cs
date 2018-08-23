using UnityEngine;
using UnityEngine.UI;

public enum ECardType
{
    Act,
    Buff,
    Weapon
}

public enum ECardName
{
    Appaloosa,
    Bang,
    Barrel,
    Beauty,
    Beer,
    Carabine,
    Colt,
    Duel,
    Dynamite,
    Gatling,
    Indians,
    Jail,
    Missed,
    Mustang,
    Panic,
    Rage,
    Remington,
    Saloon,
    Stagecoach,
    Store,
    Volcano,
    WellsFargo
}

public enum ECardRating
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public enum ECardSuit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public class PackAsset : ScriptableObject
{
    public Sprite PackSprite;
    public ECardType CardType;
    public ECardName CardName;
    public ECardRating CardRating;
    public ECardSuit CardSuit;
    public Button firstStageButton;
    [Header("Not always allowed")]
    public Button secondStageButton;
    public Button thirdStageButton;
}
