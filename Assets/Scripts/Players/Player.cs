using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image[] health;
    public static Image character;
    public static Image role;
    public static Image weapon;
    public static List<Sprite> hand = new List<Sprite>();
    public static List<Sprite> buffs = new List<Sprite>();
    public static bool hasScope = false;

    int maxHealth = 4;

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<Image>();
        character.sprite = ComponentPreload.GetCharacter();

        role = character = GameObject.FindGameObjectWithTag("PlayerRole").GetComponent<Image>();
        role.sprite = ComponentPreload.GetRole();

        weapon = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<Image>();

        if (character.sprite.name.Contains("paul_regred") || character.sprite.name.Contains("el_gringo")) maxHealth--;
        if (role.sprite.name.Contains("sheriff")) maxHealth++;

        hand.Add(Resources.Load<Sprite>("Images/Pack/appaloosa_ace_spades"));

        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }

    static int GetPlayerDistance()
    {
        string spreiteName = weapon.sprite.name;

        if (spreiteName.Contains("colt") && !spreiteName.Contains("colt_defalt")) return 2;
        else if (spreiteName.Contains("remington")) return 3;
        else if (spreiteName.Contains("carabine")) return 4;
        else if (spreiteName.Contains("volcano")) return 5;
        return 1;
    }
}
