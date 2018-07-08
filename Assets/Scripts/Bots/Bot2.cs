using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot2 : BotAction
{
    void Awake()
    {
        characterInfo = new CharacterInfo(this.gameObject, 2);
    }
}
