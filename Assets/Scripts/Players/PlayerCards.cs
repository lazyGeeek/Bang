using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private GameObject cardSpawn;

	private void Awake ()
    {
        foreach(Image sp in Player.hand)
        {
            Instantiate<Image>(sp, cardSpawn.transform);
        }
	}

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
