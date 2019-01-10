using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCards : MonoBehaviour
{
    public GameObject cardSpawn;
    public Button closeButton;
    public Button dropCardButton;
    public Text messageField;
    
    public void ShowPermanentMessage(string message)
    {
        messageField.text = message;

        messageField.GetComponent<CanvasGroup>().alpha = 1f;
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

        closeButton.gameObject.SetActive(true);

        if (GlobalVeriables.GameState == EGameState.Defense)
            dropCardButton.gameObject.SetActive(false);
        else
            dropCardButton.gameObject.SetActive(true);
    }

    public void DropCards()
    {
        if (GlobalVeriables.GameState == EGameState.DropCards)
        {
            GlobalVeriables.GameState = EGameState.Move;
            dropCardButton.GetComponentInChildren<Text>().text = "Drop Cards";
        }
        else if (GlobalVeriables.GameState == EGameState.Move)
        {
            GlobalVeriables.GameState = EGameState.DropCards;
            dropCardButton.GetComponentInChildren<Text>().text = "Continue Move";
        }
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
        if (GlobalVeriables.GameState == EGameState.Defense)
        {
            UIElements.Instance.Player.Hit();
            UIElements.Instance.Player.ShowBulletHole();
            GlobalVeriables.GameState = EGameState.Move;
        }

        ClearCardSpawn();
        messageField.GetComponent<CanvasGroup>().alpha = 0f;
        gameObject.SetActive(false);
    }
}
