using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour
{
    public Character[] characters;
    public Image sheriffImage;
    
	private void Start ()
    {
		foreach (Character character in characters)
        {
            character.InitiateCharacter();

            if (character.RoleInfo.Role == ERole.Sheriff)
            {
                GlobalVeriables.CurrentSheriff = character;
                GlobalVeriables.CurrentPlayer = character;
                sheriffImage.sprite = character.CharacterImage.sprite;
            }
        }

        foreach (Character character in characters)
        {
            if (character.Type == ECharacterType.Bot)
                character.FindEnemy();

            character.FillVisibility();
        }
    }

    public void StartGame()
    {
        GlobalVeriables.CurrentPlayer.StartMove();
    }
}