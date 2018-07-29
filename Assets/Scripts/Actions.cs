using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public void ShowCrads()
    {
        Instantiate(Resources.Load("PlayerCards"), gameObject.transform.Find("PlayTable"));
    }

    /*public static void AddBuff(Character a, Sprite s)
    {
        Canvas buffZone = a.transform.Find("BuffZone").GetComponent<Canvas>();
        Image newBuff = Instantiate(Resources.Load<Image>("ImageTemplate"), buffZone.transform);
        newBuff.sprite = s;
        newBuff.name = s.name;
    }*/

    public static int GetScope(Sprite s)
    {
        if (s.name.Contains("colt") && !s.name.Contains("colt_defalt")) return 2;
        else if (s.name.Contains("remington")) return 3;
        else if (s.name.Contains("carabine")) return 4;
        else if (s.name.Contains("volcano")) return 5;
        return 1;
    }

    //Set tag to card
    public static string GetTag(Sprite sp)
    {
        if (sp.name.Contains("volcano") || sp.name.Contains("remington") ||
            sp.name.Contains("colt") || sp.name.Contains("carabine")) return "Gun";
        else if (sp.name.Contains("appaloosa") || sp.name.Contains("barrel") ||
            sp.name.Contains("dynamite") || sp.name.Contains("jail") ||
            sp.name.Contains("mustang") || sp.name.Contains("rage")) return "Buff";
        return "Act";
    }
}
