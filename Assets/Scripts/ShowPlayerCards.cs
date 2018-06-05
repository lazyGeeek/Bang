using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerCards : MonoBehaviour
{
    [SerializeField] private GameObject playerCardsZone;
    [SerializeField] private GameObject parentObject;
    
    //Show all player card in hand
    public void ShowZone()
    {
        Instantiate(playerCardsZone.gameObject, parentObject.transform);
    }
}
