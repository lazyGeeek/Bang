using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Health")]
    public Image[] Health;
    
    [Header("Character")]
    public Image CharacterImage;
    public Image JailImage;
    public CanvasGroup BulletHole;
    public CanvasGroup Barell;

    [Header("Role")]
    public Image RoleImage;
    public Image BackRoleImage;

    [Header("Weapon")]
    public Image WeaponImage;
    public Image DefaultWeaponImage;

    [Header("Other")]
    public Image ImageOfDesk;
    public GameObject BuffZone;
    public Character NextPlayer;
    public Character PreviousPlayer;

    [System.NonSerialized] public List<PackAsset> Hand = new List<PackAsset>();
    [System.NonSerialized] public List<PackAsset> Buffs = new List<PackAsset>();
    public Dictionary<Character, int> EnemyVisibility = new Dictionary<Character, int>();

    [System.NonSerialized] public byte MaxHealth = 4;
    public bool IsDead { get; set; }
    public bool InJail { get; set; }
    public int  Scope { get; set; }
    public RoleAsset RoleInfo { get; protected set; }

    private PackAsset _weapon;
    public PackAsset Weapon
    {
        get { return _weapon; }
        set
        {
            if (_weapon != null)
                Hand.Add(_weapon);
            
            if (value == null)
            {
                Scope = 1;
                DefaultWeaponImage.gameObject.SetActive(true);
                WeaponImage = null;
                _weapon = null;
            }
            else
            {
                _weapon = value;
                WeaponImage.sprite = value.PackSprite;
                Scope = Actions.GetScope(value);
                DefaultWeaponImage.gameObject.SetActive(false);
                Hand.Remove(value);
            }
        }
    }
    
    public byte CurrentHealth
    {
        get
        {
            byte _currentHealth = 0;
            foreach (Image health in Health)
            {
                if (health.IsActive())
                    _currentHealth++;
            }
            return _currentHealth;
        }
    }

    public static bool operator ==(Character ch1, Character ch2)
    {
        return ReferenceEquals(ch1, ch2);
    }

    public static bool operator !=(Character ch1, Character ch2)
    {
        return !(ch1 == ch2);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        if (other == null) return false;
        return this == (Character)other;
    }

    public virtual void InitiateCharacter()
    {
        CharacterImage.sprite = PackAndDiscard.Instance.GetRandomCharacter();
        RoleInfo = PackAndDiscard.Instance.GetRandomRole();
        RoleImage.sprite = RoleInfo.RoleSpite;

        if (CharacterImage.sprite.name.Contains("el_gringo") || CharacterImage.sprite.name.Contains("paul_regred"))
            MaxHealth--;

        if (RoleInfo.Role == ERole.Sheriff)
            MaxHealth++;

        Scope = 1;
        IsDead = false;

        SetHealth();
    }

    public void FillVisibility()
    {
        if (this == GlobalVeriables.Instance.Player)
        {
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[0], 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[1], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[2], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[3], 1);
        }
        else if (this == GlobalVeriables.Instance.Enemies[0])
        {
            EnemyVisibility.Add(GlobalVeriables.Instance.Player, 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[1], 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[2], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[3], 2);
        }
        else if (this == GlobalVeriables.Instance.Enemies[1])
        {
            EnemyVisibility.Add(GlobalVeriables.Instance.Player, 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[0], 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[2], 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[3], 2);
        }
        else if (this == GlobalVeriables.Instance.Enemies[2])
        {
            EnemyVisibility.Add(GlobalVeriables.Instance.Player, 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[0], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[1], 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[3], 1);
        }
        else if (this == GlobalVeriables.Instance.Enemies[3])
        {
            EnemyVisibility.Add(GlobalVeriables.Instance.Player, 1);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[0], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[1], 2);
            EnemyVisibility.Add(GlobalVeriables.Instance.Enemies[2], 1);
        }
    }

    protected void SetHealth()
    {
        for (int i = 0; i < MaxHealth; i++)
        {
            Health[i].gameObject.SetActive(true);
            Hand.Add(PackAndDiscard.Instance.GetRandomCard());
        }
    }

    public virtual void AddBuff(PackAsset buff)
    {
        Buffs.Add(buff);
        Hand.Remove(buff);
        //UsedCard.Add(buff);

        GameObject image = new GameObject();
        image.transform.SetParent(BuffZone.transform, false);
        Image newBuff = image.AddComponent<Image>();
        newBuff.sprite = buff.PackSprite;

        if (buff.CardName == ECardName.Appaloosa)
            Scope++;
    }

    public void RemoveBuff(PackAsset buff)
    {
        Buffs.Remove(buff);
        
        foreach (Image buffImage in BuffZone.GetComponentsInChildren<Image>())
        {
            if (buffImage.sprite == buff.PackSprite)
                Destroy(buffImage.gameObject);
        }

        if (buff.CardName == ECardName.Appaloosa)
            Scope--;

        else if (buff.CardName == ECardName.Jail)
            JailImage.gameObject.SetActive(false);
    }

    public void Hit(Character enemy = null)
    {
        if (CurrentHealth == 0)
        {
            Debug.Log("He is already dead");
            return;
        }

        if (CurrentHealth == 1)
        {
            Health[0].gameObject.SetActive(false);
            IsDead = true;
            BackRoleImage.gameObject.SetActive(false);
            if (enemy != null)
                Death(enemy);
        }
        else
            Health[CurrentHealth - 1].gameObject.SetActive(false);
    }

    protected virtual void Death(Character enemy)
    {
        List<PackAsset> cards = new List<PackAsset>(Hand);
        
        Hand.Clear();
        cards.AddRange(Buffs);
        Buffs.Clear();

        if (Weapon != null)
            cards.Add(Weapon);

        Weapon = null;

        if (enemy != null && enemy.RoleInfo.Role == ERole.Sheriff && RoleInfo.Role == ERole.Assistant)
        {
            enemy.Hand.AddRange(cards);
        }
        else
        {
            foreach (PackAsset card in cards)
                PackAndDiscard.Instance.Discard(card);
        }

        BackRoleImage.gameObject.SetActive(false);
        IsDead = true;

        PreviousPlayer.NextPlayer = NextPlayer;

        /*foreach (Character bot in UIElements.Instance.Enemies)
        {
            bot.botEnemies.Remove(this);
        }*/
    }

    public bool Heal()
    {
        if (!Health[MaxHealth - 1].IsActive())
        {
            Health[CurrentHealth].gameObject.SetActive(true);
            return true;
        }

        return false;
    }

    public void ShowBulletHole()
    {
        StartCoroutine(ShowBulletHoleCoroutine());
    }

    private IEnumerator ShowBulletHoleCoroutine()
    {
        BulletHole.alpha = 1f;

        yield return new WaitForSeconds(1f);

        while (BulletHole.alpha > 0)
        {
            BulletHole.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    /*public IEnumerator StartMove()
    {
        ImageOfDesk.color = new Color(0.8676471f, 0.7360041f, 0.0f);
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());
        Hand.Add(PackAndDiscard.Instance.GetRandomCard());

        if (Type == ECharacterType.Bot)
        {
            yield return new WaitUntil(AIMove.StartMove);
            EndMove();
        }
        else
        {
            EndMoveBttn.gameObject.SetActive(true);
            HandBttn.gameObject.SetActive(true);
        }
    }*/

    /*public void EndMove()
    {
        if (Hand.Count > Health.Length && Type == ECharacterType.Player)
        {
            Actions.Instance.ShowPlayerCards();
            GlobalVeriables.GameState = EGameState.DropCards;
        }

        if (Type == ECharacterType.Bot && UsingCards.activeSelf)
        {
            foreach (Image card in UsingCards.GetComponentsInChildren<Image>())
                Destroy(card.gameObject);
        }

        if (Type == ECharacterType.Bot)
            UsingCards.SetActive(false);
        else
        {
            EndMoveBttn.gameObject.SetActive(false);
            HandBttn.gameObject.SetActive(false);
        }

        ImageOfDesk.color = Color.white;

        PlayersMoveQueue.StartNextPlayer();
    }*/

    /*public void UsingCard(PackAsset card)
    {
        _usingCards.SetActive(true);

        GameObject image = new GameObject();
        image.transform.SetParent(_usingCards.transform, false);
        Image newBuff = image.AddComponent<Image>();
        newBuff.sprite = card.PackSprite;
    }*/
}