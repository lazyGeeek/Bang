using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerCards : MonoBehaviour
{
    [SerializeField] private GameObject playerCards;
    [SerializeField] private GameObject parent;
    
    public void ShowZone()
    {
        Instantiate(playerCards.gameObject, parent.transform);
    }
}
