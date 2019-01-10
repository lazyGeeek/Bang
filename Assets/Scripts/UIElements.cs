using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour
{
    public Character Player;
    public List<Character> Enemies;
    public ShowCards CardZone;
    public ShowCurrentPlayer CurrentPlayerZone;
    public Sprite BackCard;
    public AudioSource audioSource;

    public List<PackAsset> testCard;

    public static UIElements Instance;

    private void Awake()
    {
        Instance = this;
    }
}
