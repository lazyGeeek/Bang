using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIMove
{
    static Bot _bot;

    public static IEnumerator StartMove()
    {
        _bot = (Bot)GlobalVeriables.CurrentPlayer;
        
        _LowHealth();
        yield return new WaitForSeconds(1f);
        _SearchGunAndBuffs();
        yield return new WaitForSeconds(1f);
        _Panic();
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => GlobalVeriables.Instance.CardZone.isActiveAndEnabled);
        _Beauty();
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => GlobalVeriables.Instance.CardZone.isActiveAndEnabled);
        _GatlingAndIndians();
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => GlobalVeriables.Instance.CardZone.isActiveAndEnabled);
        yield return new WaitWhile(() => GlobalVeriables.Instance.DeadMessageZone.isActiveAndEnabled);
        _Duel();
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => GlobalVeriables.Instance.CardZone.isActiveAndEnabled);
        yield return new WaitWhile(() => GlobalVeriables.Instance.DeadMessageZone.isActiveAndEnabled);
        _Jail();
        yield return new WaitForSeconds(1f);
        _Dynamite();
        yield return new WaitForSeconds(1f);
        _CheckHealth();
        yield return new WaitForSeconds(1f);
        _AttackSomeOne();
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => GlobalVeriables.Instance.CardZone.isActiveAndEnabled);
        yield return new WaitWhile(() => GlobalVeriables.Instance.DeadMessageZone.isActiveAndEnabled);
        _EndMove();
        yield return new WaitForSeconds(1f);
    }

    private static void _LowHealth()
    {
        if (_bot.CurrentHealth > 2)
            return;

        PackAsset stagecoach = _bot.Hand.Find(card => card.CardName == ECardName.Stagecoach);
        PackAsset wellsFargo = _bot.Hand.Find(card => card.CardName == ECardName.WellsFargo);
        
        if (wellsFargo != null)
        {
            GetCardsLogic.StageCoach(_bot, wellsFargo);
            _bot.UsingCard(wellsFargo);
        }
        else if (stagecoach != null)
        {
            GetCardsLogic.StageCoach(_bot, stagecoach);
            _bot.UsingCard(stagecoach);
        }

        PackAsset store = _bot.Hand.Find(card => card.CardName == ECardName.Store);

        if (store != null)
        {
            StoreLogic.Store(_bot, store);
            _bot.UsingCard(store);
        }

        PackAsset saloon = _bot.Hand.Find(card => card.CardName == ECardName.Saloon);

        if (_bot.CurrentHealth == 1 && saloon != null)
        {
            SaloonLogic.Saloon(_bot, saloon);
            _bot.UsingCard(saloon);
        }
    }

    private static void _Panic()
    {
        PackAsset panic = _bot.Hand.Find(card => card.CardName == ECardName.Panic);
        
        if (panic == null)
            return;

        List<Character> enemies = new List<Character>(Actions.GetScopeEnemies(_bot, true));

        foreach (Character enemy in enemies)
        {
            if (_bot.botEnemies.Contains(enemy) && enemy.Hand.Count > 0)
            {
                PanicLogic.Panic(_bot, enemy, panic);
                _bot.UsingCard(panic);
                break;
            }
        }
    }

    private static void _Beauty()
    {
        PackAsset beauty = _bot.Hand.Find(card => card.CardName == ECardName.Beauty);
        
        if (beauty == null || _bot.botEnemies.Count == 0)
            return;

        BeautyLogic.Beauty(_bot, _bot.botEnemies[Random.Range(0, _bot.botEnemies.Count)], beauty);
        _bot.UsingCard(beauty);
    }

    private static void _GatlingAndIndians()
    {
        if (_bot.RoleInfo.Role == ERole.Assistant)
            return;

        PackAsset gatling = _bot.Hand.Find(card => card.CardName == ECardName.Gatling);

        if (gatling == null)
            return;

        _bot.StartCoroutine(GatlingLogic.Gatling(_bot, (GatlingLogic)gatling));
        _bot.UsingCard(gatling);

        PackAsset indians = _bot.Hand.Find(card => card.CardName == ECardName.Indians);

        if (indians == null)
            return;

        _bot.StartCoroutine(IndiansLogic.Indians(_bot, (IndiansLogic)indians));
        _bot.UsingCard(indians);
    }

    private static void _Duel()
    {
        if (_bot.botEnemies.Count == 0) return;

        PackAsset duel = _bot.Hand.Find(card => card.CardName == ECardName.Duel);

        if (duel == null) return;

        DuelLogic.Duel(_bot, _bot.botEnemies[Random.Range(0, _bot.botEnemies.Count)], duel);
        _bot.UsingCard(duel);
    }

    private static void _SearchGunAndBuffs()
    {
        List<PackAsset> cards = new List<PackAsset>(_bot.Hand.FindAll(card => card.CardType == ECardType.Weapon));
        cards.AddRange(_bot.Hand.FindAll(card => card.CardType == ECardType.Buff));

        if (cards.Count == 0)
            return;

        foreach (PackAsset card in cards)
        {
            if (card.CardType == ECardType.Weapon && Actions.GetScope(card) > Actions.GetScope(_bot.Weapon))
            {
                _bot.Weapon = card;
                _bot.UsingCard(card);
            }
            else if (card.CardName == ECardName.Appaloosa ||
                     card.CardName == ECardName.Mustang ||
                     card.CardName == ECardName.Rage ||
                     card.CardName == ECardName.Barrel)
            {
                if (!_bot.Buffs.Find(buff => buff.CardName == card.CardName))
                {
                    _bot.AddBuff(card);
                    _bot.UsingCard(card);
                }
            }
        }
    }

    private static void _Jail()
    {
        if (_bot.botEnemies.Count == 0) return;

        PackAsset jail = _bot.Hand.Find(card => card.CardName == ECardName.Jail);

        if (jail == null) return;

        JailLogic.Jail(_bot, _bot.botEnemies[Random.Range(0, _bot.botEnemies.Count)], jail);
        _bot.UsingCard(jail);
    }

    private static void _Dynamite()
    {
        if (_bot.RoleInfo.Role == ERole.Assistant || _bot.RoleInfo.Role == ERole.Sheriff) return;

        PackAsset dynamite = _bot.Hand.Find(card => card.CardName == ECardName.Dynamite);

        if (dynamite == null) return;
        
        _bot.AddBuff(dynamite);
        _bot.UsingCard(dynamite);
    }

    private static void _CheckHealth()
    {
        if (_bot.CurrentHealth >= _bot.MaxHealth)
            return;

        PackAsset beer = _bot.Hand.Find(card => card.CardName == ECardName.Beer);

        if (beer != null && _bot.Heal())
        {
            _bot.Hand.Remove(beer);
            PackAndDiscard.Instance.Discard(beer);
            _bot.UsingCard(beer);
        }
    }

    private static void _AttackSomeOne()
    {
        PackAsset bang = _bot.Hand.Find(card => card.CardName == ECardName.Bang);

        if (bang == null)
            return;

        if (_bot.botEnemies.Count > 0)
        {
            Character enemy = _bot.botEnemies[Random.Range(0, _bot.botEnemies.Count)];

            if (enemy == GlobalVeriables.Instance.Player)
                BangLogic.Bang((Player)enemy, _bot, (BangLogic)bang);
            else
                BangLogic.Bang((Bot)enemy, _bot, (BangLogic)bang);

            _bot.UsingCard(bang);
        }

        PackAsset rage = _bot.Buffs.Find(card => card.CardName == ECardName.Rage);

        if (rage != null)
        {
            List<Character> enemies = new List<Character>();

            foreach (Character enemy in Actions.GetScopeEnemies(_bot, true))
            {
                if (_bot.botEnemies.Contains(enemy))
                    enemies.Add(enemy);
            }

            if (_bot.Hand.FindAll(card => card.CardName == ECardName.Bang).Count > 0 && enemies.Count > 0)
            {
                while (_bot.Hand.FindAll(card => card.CardName == ECardName.Bang).Count > 0 && enemies.Count > 0)
                {
                    Character randomCharacter = enemies[Random.Range(0, enemies.Count)];
                    
                    if (randomCharacter == GlobalVeriables.Instance.Player)
                        BangLogic.Bang((Player)randomCharacter, _bot, (BangLogic)bang);
                    else
                        BangLogic.Bang((Bot)randomCharacter, _bot, (BangLogic)bang);

                    _bot.UsingCard(bang);

                    if (randomCharacter.IsDead)
                        enemies.Remove(randomCharacter);
                }
            }
        }
    }

    private static void _EndMove()
    {
        //yield return new WaitForSeconds(5f);

        if (_bot.Hand.Count <= _bot.CurrentHealth)
        {
            _bot.endMove = true;
            return;// yield break;
        }

        _bot.Hand.RemoveAll(card => card.CardType == ECardType.Weapon);

        if (_bot.Hand.Count <= _bot.CurrentHealth)
        {
            _bot.endMove = true;
            return;// yield break;
        }

        _bot.Hand.RemoveAll(card => card.CardType == ECardType.Buff);

        if (_bot.Hand.Count <= _bot.CurrentHealth)
        {
            _bot.endMove = true;
            return;// yield break;
        }

        ECardName[] cardsToDelete = new ECardName[]
        {
            ECardName.Jail,
            ECardName.Duel,
            ECardName.Indians,
            ECardName.Beauty,
            ECardName.Panic,
            ECardName.Saloon,
            ECardName.Stagecoach,
            ECardName.Store,
            ECardName.WellsFargo,
            ECardName.Bang,
            ECardName.Beer,
            ECardName.Gatling,
            ECardName.Missed
        };
        
        foreach (ECardName name in cardsToDelete)
        {
            List<PackAsset> cards = new List<PackAsset>(_bot.Hand.FindAll(card => card.CardName == name));

            foreach (PackAsset card in cards)
            {
                _bot.Hand.Remove(card);
                if (_bot.Hand.Count <= _bot.CurrentHealth)
                {
                    _bot.endMove = true;
                    return;// yield break;
                }
            }
        }

        Debug.Log(_bot.name + " can't delete cards any more, change algorithm");
        _bot.Hand.Clear();
    }
}