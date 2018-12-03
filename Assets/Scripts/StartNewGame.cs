using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour
{
    public Character[] characters; //Index == 0 - Player, 1 - Bot1, 2 - Bot2, etc.
    public Image sheriffImage;
    public GameObject thisObject;
    
	void Start ()
    {
        thisObject.SetActive(true);

		foreach (Character ch in characters)
        {
            ch.InitiateCharacter();

            if (ch.RoleInfo.Role == ERole.Sheriff)
            {
                GlobalVeriables.CurrentSheriff = ch;
                GlobalVeriables.CurrentPlayer = ch;
                sheriffImage.sprite = ch.CharacterImage.sprite;
                ch.StartMove();
            }
        }
	}
}