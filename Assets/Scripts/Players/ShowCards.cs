using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ShowCards : MonoBehaviour
{
    public GameObject cardSpawn;
    public Button closeButton;
    public Text messageField;

    public static ShowCards Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        messageField.text = message;

        messageField.GetComponent<CanvasGroup>().alpha = 1f;

        yield return new WaitForSeconds(1f);

        while (messageField.GetComponent<CanvasGroup>().alpha > 0)
        {
            messageField.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ShowCardSpawn()
    {
        gameObject.SetActive(true);
    }

    public void ClearCardSpawn()
    {
        foreach (Button button in cardSpawn.GetComponentsInChildren<Button>())
            Destroy(button.gameObject);

        foreach (Image image in cardSpawn.GetComponentsInChildren<Image>())
            Destroy(image.gameObject);
    }

    //Close(delete) zone
    public void Close()
    {
        if (GlobalVeriable.GameState == EGameState.Defense)
        {
            UIElements.Instance.Player.Hit();
            GlobalVeriable.GameState = EGameState.Move;
        }

        ClearCardSpawn();
        gameObject.SetActive(false);
    }
}
