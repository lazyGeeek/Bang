using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentPreload
{
    public static List<Sprite> characters = new List<Sprite>(Resources.LoadAll<Sprite>("Images/Characters"));
    public static List<Sprite> roles = new List<Sprite>(Resources.LoadAll<Sprite>("Images/Roles"));

    //Randomly set character
    public static Sprite GetCharacter()
    {
        Sprite ch = characters[Random.Range(0, characters.Count)];
        characters.Remove(ch);
        return ch;
    }

    //Randomly set role
    public static Sprite GetRole()
    {
        Sprite rl = roles[Random.Range(0, roles.Count)];
        roles.Remove(rl);
        return rl;
    }
}
