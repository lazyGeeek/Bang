using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterName
{
    BartCassidy,
    BlackJack,
    CalamityJanet,
    ElGringo,
    JesseJones,
    Jourdonnais,
    KitCarlson,
    LuckyDuke,
    PaulRegred,
    PedroRamirez,
    RoseDoolan,
    SidKetchum,
    SlabTheKiller,
    SuzyLafayette,
    VultureSam,
    WillyTheKid
}

public class CharacterAsset : ScriptableObject
{
    public Sprite CharacterSprite;
    public ECharacterName Name;
    public int MaxHealth;
}
