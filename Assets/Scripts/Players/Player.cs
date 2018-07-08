using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static CharacterInfo characterInfo;

    private void Awake()
    {
        characterInfo = new CharacterInfo(this.gameObject);

        //For test
        characterInfo.hand.Add(Resources.Load<Sprite>("Images/Pack/appaloosa_ace_spades"));
        characterInfo.hand.Add(Resources.Load<Sprite>("Images/Pack/barrel_king_spades"));
        characterInfo.hand.Add(Resources.Load<Sprite>("Images/Pack/dynamite_2_hearts"));
        characterInfo.hand.Add(Resources.Load<Sprite>("Images/Pack/mustang_8_hearts"));
        characterInfo.hand.Add(Resources.Load<Sprite>("Images/Pack/rage_10_clubs"));
    }

    //Set maximum range to distance
    static int GetPlayerDistance()
    {
        string spreiteName = characterInfo.weapon.sprite.name;

        if (spreiteName.Contains("colt") && !spreiteName.Contains("colt_defalt")) return 2;
        else if (spreiteName.Contains("remington")) return 3;
        else if (spreiteName.Contains("carabine")) return 4;
        else if (spreiteName.Contains("volcano")) return 5;
        return 1;
    }
}
