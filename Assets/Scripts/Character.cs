using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public struct CharacterInfo
{
    public Image[] health;
    public Image character;
    public Image role;
    public Image weapon;
    public List<Sprite> hand;
    public List<Sprite> buffs;

    public int maxHealth;
    public int scope;
    public int position;
    public bool hasShield;
    public bool onHorse;
    public bool inJail;
    public bool inRage;
    
}*/

public class Character : MonoBehaviour
{
    //CharacterInfo characterInfo;
    Image[] health;
    Image character;
    Image role;
    Image weapon;
    List<Sprite> hand;
    List<Sprite> buffs;

    int maxHealth;
    int scope;
    int position;
    bool hasShield;
    bool onHorse;
    bool inJail;
    bool inRage;

    public Sprite Weapon
    {
        get { return weapon.sprite; }
        set { weapon.sprite = value; }
    }

    public int Scope
    {
        get { return scope; }
        set { scope = value; }
    }

    public int Position
    {
        get { return position; }
        set { position = value; }
    }

    public bool InJail
    {
        get { return inJail; }
        set { inJail = value; }
    }

    public List<Sprite> Hand
    {
        get { return hand; }
        set { hand = value; }
    }

    public List<Sprite> Buffs
    {
        get { return buffs; }
        set { buffs = value; }
    }

    public bool HasShield
    {
        get { return hasShield; }
        set { hasShield = value; }
    }

    public bool OnHorse
    {
        get { return onHorse; }
        set { onHorse = value; }
    }

    public bool InRage
    {
        get { return inRage; }
        set { inRage = value; }
    }

    protected void Awake()
    {
        health = new Image[5];
        for (int i = 0; i < health.Length; ++i)
        {
            health[i] = gameObject.transform.Find("Health" + (i + 1).ToString()).GetComponent<Image>();
        }

        character = gameObject.transform.Find("Character").GetComponent<Image>();
        character.sprite = ComponentPreload.GetCharacter();

        role = gameObject.transform.Find("Role").GetComponent<Image>();
        role.sprite = ComponentPreload.GetRole();

        if (role.sprite.name.Contains("sheriff") && !gameObject.name.Contains("Player"))
            role.transform.Find("Back").gameObject.SetActive(false);

        weapon = gameObject.transform.Find("Weapon").GetComponent<Image>();

        hand = new List<Sprite>();
        buffs = new List<Sprite>();

        maxHealth = 4;
        scope = 1;
        hasShield = false;
        onHorse = false;
        inJail = false;
        inRage = false;
        position = 0;

        //SetMaxHealth();
        if (character.sprite.name.Contains("paul_regred") ||
            character.sprite.name.Contains("el_gringo")) maxHealth--;
        if (role.sprite.name.Contains("sheriff")) maxHealth++;

        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }

    public void ShowScard(GridLayoutGroup cardSpawn)
    {
        foreach (Sprite sp in Hand)
        {
            Button button = Instantiate(Resources.Load<Button>("playerCardButton"), cardSpawn.transform);
            button.image.sprite = sp;
            button.tag = Actions.GetTag(sp);
            button.name = sp.name;
            button.GetComponent<Pack>().character = this;
        }
    }
}


