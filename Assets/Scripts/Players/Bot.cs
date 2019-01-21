using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : Character
{
    public GameObject UsingCards;

    [System.NonSerialized]
    public List<Character> botEnemies = new List<Character>();
    [System.NonSerialized]
    public bool endMove = false;

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
        if (RoleInfo.Role == ERole.Sheriff)
        {
            if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
                GlobalVeriables.Instance.CardZone.Close();
            GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("Sheriff is dead! Bad guys are win!");
            return;
        }

        GlobalVeriables.Instance.Enemies.Remove(this);

        List<Character> alivePeople = new List<Character> { GlobalVeriables.Instance.Player };
        GlobalVeriables.Instance.Enemies.ForEach(bot => alivePeople.Add(bot));
        alivePeople.RemoveAll(player => player.RoleInfo.Role == ERole.Assistant || player.RoleInfo.Role == ERole.Sheriff);


        if (alivePeople.Count == 0)
        {
            if (GlobalVeriables.Instance.CardZone.isActiveAndEnabled)
                GlobalVeriables.Instance.CardZone.Close();
            GlobalVeriables.Instance.DeadMessageZone.ShowDeadMessage("All bad guys are dead! Good guys are win!");
            return;
        }

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

        StartCoroutine(AIMove.StartMove());
        yield return new WaitUntil(() => endMove);

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
        endMove = true;

        StartCoroutine(PlayersMoveQueue.StartNextPlayer());
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
