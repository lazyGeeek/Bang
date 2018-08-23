using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingLogic : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioS;

    public static GatlingLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGatling(Character init)
    {
        audioS.Play();

        if (UIElements.Instance.Player != init && !UIElements.Instance.Player.IsDead)
            UIElements.Instance.Player.Hit();

        foreach (Character enemy in UIElements.Instance.Enemies)
        {
            if (enemy != init && !enemy.IsDead)
                enemy.Hit();
        }
    }
}
