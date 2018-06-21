using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour
{
    Button currentButton;

    //Checking card tag (gun, buff, etc.)
    public void CheckTag(Button btn)
    {
        currentButton = btn;

        switch (this.tag)
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

        if (!Player.weapon.sprite.name.Contains("colt_default"))
        {
            Player.hand.Add(Player.weapon.sprite);
            currentButton.image.sprite = Player.weapon.sprite;
        }
        else
            Destroy(this.gameObject);

        Player.scope -= GetScope(Player.weapon.sprite.name);
        Player.weapon.sprite = tempSprite;
        Player.hand.Remove(tempSprite);
        Player.scope += GetScope(Player.weapon.sprite.name);
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
            SetBuffToPlayer();
            Player.scope++;
        }
        else if (spriteName.Contains("barrel"))
        {
            SetBuffToPlayer();
            Player.hasShield = true;
        }
        else if (spriteName.Contains("mustang"))
        {
            SetBuffToPlayer();
            Player.onHorse = true;
        }
        else if (spriteName.Contains("jail"))
        {

        }
        else if (spriteName.Contains("dynamite"))
        {
            SetBuffToPlayer();
        }
        else if (spriteName.Contains("rage"))
        {
            SetBuffToPlayer();
            Player.inRage = true;
        }
    }

    //Add buff to buff zone
    private void SetBuffToPlayer()
    {
        Canvas playerBuffZone = GameObject.FindGameObjectWithTag("PlayerBuffs").GetComponent<Canvas>();
        Image newBuff = Instantiate(Player.weapon, playerBuffZone.transform);
        newBuff.sprite = currentButton.image.sprite;
        newBuff.name = newBuff.sprite.name;
        Player.buffs.Add(newBuff.sprite);
        Player.hand.Remove(newBuff.sprite);
        Destroy(this.gameObject);
    }
}
