using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CharacterInfo
{
    public Image[] health;
    public Image character;
    public Image role;
    public Image weapon;
    public List<Sprite> hand;
    public List<Sprite> buffs;

    public int maxHealth;
    public int scope;
    public bool hasShield;
    public bool onHorse;
    public bool inJail;
    public bool inRage;
    public int position;

    public CharacterInfo(GameObject g, int p = 0)
    {
        health = new Image[5];
        for (int i = 0; i < health.Length; ++i)
        {
            health[i] = g.transform.Find("Health" + (i + 1).ToString()).GetComponent<Image>();
        }

        character = g.transform.Find("Character").GetComponent<Image>();
        character.sprite = ComponentPreload.GetCharacter();

        role = g.transform.Find("Role").GetComponent<Image>();
        role.sprite = ComponentPreload.GetRole();
        if (role.sprite.name.Contains("sheriff"))
            role.transform.Find("Back").gameObject.SetActive(false);

        weapon = g.transform.Find("Weapon").GetComponent<Image>();

        hand = new List<Sprite>();
        buffs = new List<Sprite>();

        maxHealth = 4;
        scope = 1;
        hasShield = false;
        onHorse = false;
        inJail = false;
        inRage = false;
        position = p;

        SetMaxHealth();

        AddCards();
    }

    void SetMaxHealth()
    {
        if (character.sprite.name.Contains("paul_regred") || character.sprite.name.Contains("el_gringo")) maxHealth--;
        if (role.sprite.name.Contains("sheriff")) maxHealth++;
    }

    void AddCards()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }
}
