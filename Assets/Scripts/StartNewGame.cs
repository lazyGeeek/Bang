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

        foreach (Bot bot in GlobalVeriables.Instance.Enemies)
        {
            bot.FindEnemy();
            bot.FillVisibility();
        }

        GlobalVeriables.Instance.Player.FillVisibility();
    }

    public void StartGame()
    {
        if (GlobalVeriables.CurrentPlayer == GlobalVeriables.Instance.Player)
            ((Player)GlobalVeriables.CurrentPlayer).StartMove();
        else
            GlobalVeriables.CurrentPlayer.StartCoroutine(((Bot)GlobalVeriables.CurrentPlayer).StartMove());
    }
}