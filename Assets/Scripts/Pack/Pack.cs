using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour
{
    [SerializeField] Button currentButton;
    private Character character;
    private PlayerCards cardSpawn;

    public Character Charact
    {
        get { return character; }
        set { character = value; }
    }

    public PlayerCards CardSpawn
    {
        get { return cardSpawn; }
        set { cardSpawn = value; }
    }

    //Checking card tag (gun, buff, etc.)
    public void CheckTag()
    {
        switch (tag)
        {
            case "Gun":
                ChangePlayerGun();
                break;
            case "Buff":
                CheckBuff();
                break;
            case "Act":
                CheckAction();
                break;
            default:
                Debug.LogError("Incorrect tag");
                break;
        }
    }
    
    //Change player weapon
    private void ChangePlayerGun()
    {
        Sprite tempSprite = currentButton.image.sprite;

        if (!character.Weapon.name.Contains("colt_default"))
        {
            character.Hand.Add(character.Weapon);
            currentButton.image.sprite = character.Weapon;
        }
        else
            Destroy(gameObject);
        
        character.Weapon = tempSprite;
        character.Hand.Remove(tempSprite);
        character.Scope = Actions.GetScope(character.Weapon);
    }

    //Check what kind of buff
    private void CheckBuff()
    {
        string spriteName = currentButton.image.sprite.name;

        if (spriteName.Contains("appaloosa"))
        {
            SetBuff();
            character.Scope++;
        }
        else if (spriteName.Contains("barrel"))
        {
            SetBuff();
            character.HasShield = true;
        }
        else if (spriteName.Contains("mustang"))
        {
            SetBuff();
            character.Position++;
        }
        else if (spriteName.Contains("jail"))
        {
            PackAndDiscard.Discard(currentButton.image.sprite);
            character.Hand.Remove(currentButton.image.sprite);
            cardSpawn.ShowEnemies(Resources.Load<Button>("jailButton"));
        }
        else if (spriteName.Contains("dynamite"))
        {
            currentButton.enabled = false;

            PackAndDiscard.Discard(currentButton.image.sprite);
            character.Hand.Remove(currentButton.image.sprite);

            foreach(Button b in cardSpawn.cardSpawn.GetComponentsInChildren<Button>())
            {
                if (b.image.sprite.name != currentButton.image.sprite.name)
                    Destroy(b.gameObject);
            }
            StartCoroutine(cardSpawn.Dynamite(0));
        }
        else if (spriteName.Contains("rage"))
        {
            SetBuff();
            character.InRage = true;
        }
    }

    private void CheckAction()
    {
        string spriteName = currentButton.image.sprite.name;

        if (spriteName.Contains("bang"))
        {
            PackAndDiscard.Discard(currentButton.image.sprite);
            character.Hand.Remove(currentButton.image.sprite);
            cardSpawn.ShowEnemiesForBang();
        }
        else if (spriteName.Contains("beauty"))
        {
            PackAndDiscard.Discard(currentButton.image.sprite);
            character.Hand.Remove(currentButton.image.sprite);
            cardSpawn.ShowEnemies(Resources.Load<Button>("beautyEnemy"));
        }
        else if (spriteName.Contains("beer"))
        {
            if (character.Heal())
            {
                PackAndDiscard.Discard(currentButton.image.sprite);
                character.Hand.Remove(currentButton.image.sprite);
                cardSpawn.Close();
            }
        }
        else if (spriteName.Contains("duel"))
        {
            PackAndDiscard.Discard(currentButton.image.sprite);
            character.Hand.Remove(currentButton.image.sprite);
            cardSpawn.ShowEnemies(Resources.Load<Button>("duelButton"));
        }
    }

    //Add buff to buff zone
    private void SetBuff()
    {
        Canvas playerBuffZone = character.transform.Find("BuffZone").GetComponent<Canvas>();
        Image newBuff = Instantiate(Resources.Load<Image>("ImageTemplate"), playerBuffZone.transform);
        newBuff.sprite = currentButton.image.sprite;
        newBuff.name = newBuff.sprite.name;
        character.Buffs.Add(newBuff.sprite);
        character.Hand.Remove(newBuff.sprite);
        Destroy(gameObject);
    }
    
    public void Jail()
    {
        character.Jail();
        cardSpawn.Close();
    }

    public void Bang()
    {
        character.Hit();
        cardSpawn.Close();
    }

    public void Beauty()
    {
        cardSpawn.ShowEnemyCards(character);
    }

    public void DropEnemyCard()
    {
        PackAndDiscard.Discard(currentButton.image.sprite);
        character.Hand.Remove(currentButton.image.sprite);
        cardSpawn.Close();
    }

    public void StartDuel()
    {
        cardSpawn.Close();
        character.Duel(cardSpawn.players[0]);
    }
}
