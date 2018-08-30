using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    Defense, //If someone attack
    Move
}

public class GlobalVeriable
{
    public static Character CurrentEnemy;
    public static PackAsset CurrentCard;
    public static EGameState GameState = EGameState.Move;
    public static bool IsDynamite = false;
    public static Character DynamiteInit;
}
