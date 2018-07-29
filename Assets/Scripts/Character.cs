using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    int scope; //Scope of gun
    [SerializeField] int position; //Enemy position relative to the player
    bool hasShield;
    //bool onHorse;
    bool inJail;
    bool inRage;
    bool isDead;

    public Sprite Weapon
    {
        get { return weapon.sprite; }
        set { weapon.sprite = value; }
    }

    public Sprite CharacterSprite
    {
        get { return character.sprite; }
        set { character.sprite = value; }
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

    /*public bool OnHorse
    {
        get { return onHorse; }
        set { onHorse = value; }
    }*/

    public bool InRage
    {
        get { return inRage; }
        set { inRage = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
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

        hand  = new List<Sprite>();
        buffs = new List<Sprite>();

        maxHealth = 4;
        scope     = 1;
        //position  = SetPosition();
        hasShield = false;
        //onHorse   = false;
        inJail    = false;
        inRage    = false;
        isDead    = false;

        //SetMaxHealth();
        if (character.sprite.name.Contains("paul_regred") ||
            character.sprite.name.Contains("el_gringo")) maxHealth--;
        if (role.sprite.name.Contains("sheriff")) maxHealth++;

        //For test
        if (name.Contains("Player"))
        {
            Hand.Add(Resources.Load<Sprite>("Images/Pack/duel_8_clubs"));
        }

        SetHealth();
        /*for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }*/

        
    }

    void SetHealth()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            hand.Add(PackAndDiscard.GetCard());
        }
    }

    int SetPosition()
    {
        if (name.Contains("Bot1") || name.Contains("Bot4"))
            return 1;
        else if (name.Contains("Bot2") || name.Contains("Bot3"))
            return 2;
        return 0;
    }

    public void Hit()// Complete this
    {
        if (!isDead)
        {
            for (int i = maxHealth - 1; i >= 0; --i)
            {
                if (!health[1].IsActive() && health[0].IsActive())
                {
                    health[0].gameObject.SetActive(false);
                    isDead = true;

                    if (!name.Contains("Player"))
                        role.GetComponentInChildren<Image>().gameObject.SetActive(false);

                    //Complete for death

                    break;
                }
                else if (health[i].IsActive())
                {
                    health[i].gameObject.SetActive(false);
                    break;
                }
                else if (!health[0].IsActive())
                {
                    Debug.Log("He already dead");
                }
            }
        }
    }

    public bool Heal()
    {
        if (!health[maxHealth - 1].IsActive())
        {
            foreach (Image h in health)
            {
                if (!h.IsActive())
                {
                    h.gameObject.SetActive(true);
                    return true;
                }
            }
        }

        return false;
    }

    public void Jail()
    {
        inJail = true;
        character.transform.Find("Jail").gameObject.SetActive(true);
    }

    /*public void Duel(Character enemy)
    {
        PlayerCards cardSpawn = GameObject.Find("ShowCards").GetComponent<PlayerCards>();
        cardSpawn.gameObject.SetActive(true);

        int playerCount = Hand.FindAll(l => l.name.Contains("bang")).Count;
        int enemyCount  = enemy.Hand.FindAll(l => l.name.Contains("bang")).Count;

        if (playerCount < enemyCount)
            Hit();
        else
            enemy.Hit();

        //Add draw later
    }*/

    public void ShowCards(PlayerCards cardSpawn)
    {
        foreach (Sprite sp in Hand)
        {
            Button button = Instantiate(Resources.Load<Button>("playerCardButton"), 
                                        cardSpawn.cardSpawn.transform);

            button.image.sprite = sp;
            button.tag = Actions.GetTag(sp);
            button.name = sp.name;
            button.GetComponent<Pack>().Charact = this;
            button.GetComponent<Pack>().CardSpawn = cardSpawn;
        }
    }
}


