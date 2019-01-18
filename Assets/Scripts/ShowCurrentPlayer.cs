using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentPlayer : MonoBehaviour
{
    public Image PlayerImage;

	public IEnumerator ShowPlayer(Image playerImage)
    {
        gameObject.SetActive(true);
        PlayerImage.sprite = playerImage.sprite;
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        yield return new WaitForSeconds(1f);

        while (gameObject.GetComponent<CanvasGroup>().alpha > 0)
        {
            gameObject.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        gameObject.SetActive(false);
    }
}
