using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    public GameObject cardSpawn;
    public Button closeButton;
    public Character[] players;

    public IEnumerator Dynamite(int pos)
    {
        int i = pos;
        Regex reg = new Regex(@"(\w+)_[23456789]_spades");

        Button player = Instantiate(Resources.Load<Button>("playerCardButton"), cardSpawn.transform);
        player.enabled = false;

        Button card = Instantiate(Resources.Load<Button>("playerCardButton"), cardSpawn.transform);
        card.enabled = false;

        closeButton.gameObject.SetActive(false);

        while (true)
        {
            player.image.sprite = players[i].CharacterSprite;
            card.image.sprite = PackAndDiscard.GetCard();

            if (reg.IsMatch(card.image.sprite.name))
            {
                for (int j = 0; j < 3; ++j)
                    players[i].Hit();

                yield return new WaitForSeconds(2f);
                closeButton.gameObject.SetActive(true);
                PackAndDiscard.Discard(card.image.sprite);
                Close();
                yield break;
            }

            do
            {
                if (++i == players.Length)
                    i = 0;
            } while (players[i].IsDead);

            PackAndDiscard.Discard(card.image.sprite);

            yield return new WaitForSeconds(2f);
        }
    }

    public void ShowEnemies(Button template)
    {
        closeButton.gameObject.SetActive(false);
        ClearCardSpawn();

        for (int i = 1; i < players.Length; ++i)
        {
            if (!players[i].IsDead)
            {
                Button player = Instantiate(template, cardSpawn.transform);
                player.image.sprite = players[i].CharacterSprite;
                player.GetComponent<Pack>().Charact = players[i];
                player.GetComponent<Pack>().CardSpawn = this;
            }
        }
    }

    public void ShowEnemiesForBang()
    {
        closeButton.gameObject.SetActive(false);
        ClearCardSpawn();

        for (int i = 1; i < players.Length; ++i)
        {
            if (!players[i].IsDead && 
                 players[0].Scope >= players[i].Position)
            {
                Button player = Instantiate(Resources.Load<Button>("bangButton"), cardSpawn.transform);
                player.image.sprite = players[i].CharacterSprite;
                player.GetComponent<Pack>().Charact = players[i];
                player.GetComponent<Pack>().CardSpawn = this;
            }
        }
    }

    public void ShowEnemyCards(Character c)
    {
        ClearCardSpawn();

        foreach (Sprite card in c.Hand)
        {
            Button enemyCard = Instantiate(Resources.Load<Button>("beautyEnemyCard"), cardSpawn.transform);
            enemyCard.image.sprite = card;
            enemyCard.GetComponent<Pack>().Charact = c;
            enemyCard.GetComponent<Pack>().CardSpawn = this;
        }
    }

    public void ClearCardSpawn()
    {
        foreach (Button b in cardSpawn.GetComponentsInChildren<Button>())
            Destroy(b.gameObject);
    }

    //Close(delete) zone
    public void Close()
    {
        closeButton.gameObject.SetActive(true);
        ClearCardSpawn();
        gameObject.SetActive(false);
    }
}
