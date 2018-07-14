using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour
{
    [SerializeField] Button currentButton;
    public Character character;

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
            Destroy(this.gameObject);

        character.Scope -= Actions.GetScope(character.Weapon);
        character.Weapon = tempSprite;
        character.Hand.Add(tempSprite);
        character.Scope += Actions.GetScope(character.Weapon);
    }

    int GetScope(string name)
    {
        if (name.Contains("colt"))
        {
            if (name.Contains("colt_default"))
                return 1;
            else
                return 2;
        }
        else if (name.Contains("remington"))
            return 3;
        else if (name.Contains("carabine"))
            return 4;
        else if (name.Contains("volcano"))
            return 5;
        return 1;
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
            character.OnHorse = true;
        }
        else if (spriteName.Contains("jail"))
        {

        }
        else if (spriteName.Contains("dynamite"))
        {
            SetBuff();
        }
        else if (spriteName.Contains("rage"))
        {
            SetBuff();
            character.InRage = true;
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
        Destroy(this.gameObject);
    }
}
