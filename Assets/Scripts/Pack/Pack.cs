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

        Player.weapon.sprite = tempSprite;
        Player.hand.Remove(tempSprite);
    }

    //Check what kind of buff
    private void CheckBuff()
    {
        string spriteName = currentButton.image.sprite.name;

        if (spriteName.Contains("appaloosa"))
            SetAppaloosa();
        else if (spriteName.Contains("barrel")) { }
        else if (spriteName.Contains("mustang")) { }
        else if (spriteName.Contains("jail")) { }
        else if (spriteName.Contains("dynamite")) { }
    }

    //Add appaloosa to buff zone
    private void SetAppaloosa()
    {
        for (int i = 0; i < Player.buffs.Count; ++i)
            Debug.Log(Player.buffs[i].name);
        Player.hasScope = true;
        Canvas playerBuffZone = GameObject.FindGameObjectWithTag("PlayerBuffs").GetComponent<Canvas>();
        Image newBuff = Instantiate(Player.weapon, playerBuffZone.transform);
        newBuff.sprite = currentButton.image.sprite;
        newBuff.name = newBuff.sprite.name;
        Player.buffs.Add(newBuff.sprite);
        Player.hand.Remove(newBuff.sprite);
        Destroy(this.gameObject);
    }
}
