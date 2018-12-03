using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    Defense, //If someone attack
    Move
}

public class GlobalVeriables
{
    public static Character CurrentPlayer;
    public static Character CurrentSheriff;
    public static PackAsset CurrentCard;
    public static EGameState GameState = EGameState.Move;
    public static bool IsDynamite = false;
    public static Character DynamiteInit;
}
