using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadMessage : MonoBehaviour
{
    public Text TextField;

	public void ShowDeadMessage(string message)
    {
        gameObject.SetActive(true);
        TextField.text = message;
    }
}
