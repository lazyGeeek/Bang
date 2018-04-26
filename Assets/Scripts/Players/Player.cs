using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image[] health;
    public Image character;
    public Image role;
    public static List<Image> hand = new List<Image>();

    int maxHealth = 4;

    private void Awake()
    {
        character.sprite = ComponentPreload.GetCharacter();
        role.sprite = ComponentPreload.GetRole();
        if (character.sprite.name.Contains("paul_regred") || character.sprite.name.Contains("el_gringo")) maxHealth--;
        if (role.sprite.name.Contains("sheriff")) maxHealth++;

        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }
}
