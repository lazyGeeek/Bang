using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVeriables : MonoBehaviour
{
    public Player Player;
    public List<Bot> Enemies;
    public ShowCards CardZone;
    public ShowCurrentPlayer CurrentPlayerZone;
    public DeadMessage DeadMessageZone;
    public Sprite BackCard;
    public AudioSource audioSource;

    public static Character CurrentPlayer;
    public static Character CurrentSheriff;
    public static PackAsset CurrentCard;
    public static EGameState GameState = EGameState.Move;

    public static GlobalVeriables Instance;

    private void Awake()
    {
        Instance = this;
    }
}
