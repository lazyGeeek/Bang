using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : Character
{
    public GameObject UsingCards;

    [System.NonSerialized]
    public List<Character> botEnemies = new List<Character>();

    public override void InitiateCharacter()
    {
        base.InitiateCharacter();

        if (RoleInfo.Role == ERole.Sheriff)
            BackRoleImage.gameObject.SetActive(false);
    }

    public void FindEnemy()
    {
        if (RoleInfo.Role == ERole.Assistant)
        {
            if (GlobalVeriables.Instance.Player.RoleInfo.Role != ERole.Sheriff)
                botEnemies.Add(GlobalVeriables.Instance.Player);

            foreach (Character enemy in GlobalVeriables.Instance.Enemies)
            {
                if (enemy != this && enemy.RoleInfo.Role != ERole.Sheriff)
                    botEnemies.Add(GlobalVeriables.Instance.Player);
            }
        }
        else if (RoleInfo.Role == ERole.Renegade)
        {
            botEnemies.Add(GlobalVeriables.Instance.Player);

            foreach (Character enemy in GlobalVeriables.Instance.Enemies)
            {
                if (enemy != this)
                    botEnemies.Add(GlobalVeriables.Instance.Player);
            }
        }
    }

    protected override void Death(Character enemy)
    {
        base.Death(enemy);

        foreach (Bot bot in GlobalVeriables.Instance.Enemies)
        {
            bot.botEnemies.Remove(this);
        }
    }

    public IEnumerator StartMove()
    {
        ImageOfDesk.color = new Color(0.8676471f, 0.7360041f, 0.0f);
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());

        AIMove.StartMove();
        yield return new WaitWhile(() => UsingCards.activeSelf);

        EndMove();
    }

    public void EndMove()
    {
        if (UsingCards.activeSelf)
        {
            foreach (Image card in UsingCards.GetComponentsInChildren<Image>())
                Destroy(card.gameObject);
        }

        UsingCards.SetActive(false);

        ImageOfDesk.color = Color.white;

        PlayersMoveQueue.StartNextPlayer();
    }

    public void UsingCard(PackAsset card)
    {
        UsingCards.SetActive(true);

        GameObject image = new GameObject();
        image.transform.SetParent(UsingCards.transform, false);
        Image newBuff = image.AddComponent<Image>();
        newBuff.sprite = card.PackSprite;
    }
}
