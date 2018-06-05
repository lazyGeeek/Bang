using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : MonoBehaviour
{
    public Image[] healthImage;
    public Image characterImage;
    public Image roleImage;
    public List<Sprite> hand = new List<Sprite>();

    Sprite role;
    int maxHealth = 4;

    private void Awake()
    {
        characterImage.sprite = ComponentPreload.GetCharacter();
        role = ComponentPreload.GetRole();
        if (role.name.Contains("sheriff")) roleImage.sprite = role;
        if (characterImage.sprite.name.Contains("paul_regred") || characterImage.sprite.name.Contains("el_gringo")) maxHealth--;
        if (roleImage.sprite.name.Contains("sheriff")) maxHealth++;

        for (int i = 0; i < maxHealth; i++)
        {
            healthImage[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }
}
