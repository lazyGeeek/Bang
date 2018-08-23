using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonLogic : MonoBehaviour
{
    /*public void OnSaloonClick()
    {
        Saloon(UIElements.Instance.Player);
    }*/

    public static void Saloon(Character init)
    {
        if (UIElements.Instance.Player == init)
        {
            for (int i = 0; i < 3; ++i)
            {
                UIElements.Instance.Player.Heal();
            }
        }
        else
            UIElements.Instance.Player.Heal();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy == init)
            {
                for (int i = 0; i < 3; ++i)
                {
                    UIElements.Instance.Player.Heal();
                }
            }
            else
                UIElements.Instance.Player.Heal();
        }
    }
}
