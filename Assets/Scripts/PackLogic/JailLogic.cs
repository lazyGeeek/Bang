using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailLogic : MonoBehaviour
{
    public void InJailCardClick()
    {
        if (PutInJail(GlobalVeriable.CurrentEnemy))
            UIElements.Instance.Player.RemoveCardToDiscard(GlobalVeriable.CurrentCard);
        else
            ShowCards.Instance.ShowMessage("You can't put sheriff in jail");
    }

    public static bool PutInJail(Character defendant)
    {
        if (defendant.RoleInfo.Role == ERole.Sheriff)
            return false;

        defendant.PutInJail();

        return true;
    }
}
