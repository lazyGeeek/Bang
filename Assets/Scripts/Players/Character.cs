using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Image _imageOfDesk;
    
    [SerializeField]
    private Image _jailImage;
    public Image JailImage //If player get to jail activate this object
    {
        get { return _jailImage; }
    }
    [SerializeField]
    private Image _roleImage;

    [SerializeField]
    private Image _backRoleImage;
    public Image BackRoleImage
    {
        get { return _backRoleImage; }
    }

    [SerializeField]
    private Image _weaponImage;

    [SerializeField]
    private Image _defaultWeaponImage;

    [SerializeField]
    private CanvasGroup _bulletHole;
    public CanvasGroup BulletHole
    {
        get { return _bulletHole; }
    }

    [SerializeField]
    private CanvasGroup _barell;
    public CanvasGroup Barell
    {
        get { return _barell; }
    }

    [SerializeField]
    private Image[] _health;
    public Image[] Health
    {
        get { return _health; }
    }

    //Enemy position relative to the player
    [SerializeField]
    private int _position;
    public int Position
    {
        get { return _position; }
        set { _position = value; }
    }

    [SerializeField]
    private ECharacterType _type;
    public ECharacterType Type
    {
        get { return _type; }
    }
    
    private List<PackAsset> _hand = new List<PackAsset>();
    public List<PackAsset> Hand
    {
        get { return _hand; }
    }
    
    private List<PackAsset> _buffs = new List<PackAsset>();
    public List<PackAsset> Buffs
    {
        get { return _buffs; }
    }

    [System.NonSerialized]
    public List<PackAsset> UsedCard = new List<PackAsset>(); //Used cards per turn

    //private RoleAsset _roleInfo;
    public RoleAsset RoleInfo
    {
        get;// { return _roleInfo; }
        private set;
    }

    private byte _maxHealth = 4;
    public byte MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }
    
    public bool IsDead { get; set; }
    public bool InJail { get; set; }
    public int  Scope { get; set; } //Scope of current gun
    public Image CharacterImage;
    public GameObject BuffZone; //Zone for displaying all buffs

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
                _defaultWeaponImage.gameObject.SetActive(true);
                _weaponImage = null;
                _weapon = null;
            }
            else
            {
                _weapon = value;
                _weaponImage.sprite = value.PackSprite;
                Scope = Actions.GetScope(value);
                _defaultWeaponImage.gameObject.SetActive(false);
                Hand.Remove(value);
            }
        }
    }

    private byte _currentHealth;
    public byte CurrentHealth
    {
        get
        {
            _currentHealth = 0;
            foreach (Image health in Health)
            {
                if (health.IsActive())
                    _currentHealth++;
            }
            return _currentHealth;
        }
    }
    
    [System.NonSerialized]
    public List<Character> botEnemies = new List<Character>();

    public Dictionary<Character, int> EnemyVisibility = new Dictionary<Character, int>();
    
    [SerializeField]
    private GameObject _usingCards;

    public Character NextPlayer;
    public Button EndMoveBttn;
    public Button HandBttn;

    public static bool operator ==(Character ch1, Character ch2)
    {
        return ReferenceEquals(ch1, ch2);
        /*return !ReferenceEquals(ch1, null) && 
               !ReferenceEquals(ch2, null) && 
               ch1.CharacterImage.sprite == ch2.CharacterImage.sprite &&
               ch1._roleImage.sprite == ch2._roleImage.sprite &&
               ch1._position == ch2._position;*/
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

    public void InitiateCharacter()
    {
        CharacterImage.sprite = PackAndDiscard.Instance.GetRandomCharacter();
        RoleInfo = PackAndDiscard.Instance.GetRandomRole();
        _roleImage.sprite = RoleInfo.RoleSpite;

        if (CharacterImage.sprite.name.Contains("el_gringo") || CharacterImage.sprite.name.Contains("paul_regred"))
            MaxHealth--;

        if (RoleInfo.Role == ERole.Sheriff)
            MaxHealth++;
        if (RoleInfo.Role == ERole.Sheriff || _type == ECharacterType.Player)
            _backRoleImage.gameObject.SetActive(false);

        Scope = 1;
        IsDead = false;

        _SetHealth();
    }

    public void FindEnemy()
    {
        if (RoleInfo.Role == ERole.Assistant)
        {
            if (UIElements.Instance.Player.RoleInfo.Role != ERole.Sheriff)
                botEnemies.Add(UIElements.Instance.Player);

            foreach (Character enemy in UIElements.Instance.Enemies)
            {
                if (enemy != this && enemy.RoleInfo.Role != ERole.Sheriff)
                    botEnemies.Add(UIElements.Instance.Player);
            }
        }
        else if (RoleInfo.Role == ERole.Renegade)
        {
            botEnemies.Add(UIElements.Instance.Player);

            foreach (Character enemy in UIElements.Instance.Enemies)
            {
                if (enemy != this)
                    botEnemies.Add(UIElements.Instance.Player);
            }
        }
    }

    public void FillVisibility()
    {
        if (this == UIElements.Instance.Player)
        {
            EnemyVisibility.Add(UIElements.Instance.Enemies[0], 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[1], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[2], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[3], 1);
        }
        else if (this == UIElements.Instance.Enemies[0])
        {
            EnemyVisibility.Add(UIElements.Instance.Player, 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[1], 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[2], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[3], 2);
        }
        else if (this == UIElements.Instance.Enemies[1])
        {
            EnemyVisibility.Add(UIElements.Instance.Player, 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[0], 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[2], 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[3], 2);
        }
        else if (this == UIElements.Instance.Enemies[2])
        {
            EnemyVisibility.Add(UIElements.Instance.Player, 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[0], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[1], 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[3], 1);
        }
        else if (this == UIElements.Instance.Enemies[3])
        {
            EnemyVisibility.Add(UIElements.Instance.Player, 1);
            EnemyVisibility.Add(UIElements.Instance.Enemies[0], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[1], 2);
            EnemyVisibility.Add(UIElements.Instance.Enemies[2], 1);
        }
    }

    private void _SetHealth()
    {
        for (int i = 0; i < MaxHealth; i++)
        {
            Health[i].gameObject.SetActive(true);
            Hand.Add(PackAndDiscard.Instance.GetRandomCard());
        }
    }

    public void AddCardToHand(PackAsset card)
    {
        Hand.Add(card);
    }

    public void RemoveCardFromHand(PackAsset card)
    {
        Hand.Remove(card);
    }

    public void RemoveCardToDiscard(PackAsset card)
    {
        Hand.Remove(card);
        PackAndDiscard.Instance.Discard(card);
    }

    public void AddBuff(PackAsset buff)
    {
        Buffs.Add(buff);
        Hand.Remove(buff);
        UsedCard.Add(buff);

        GameObject image = new GameObject();
        image.transform.SetParent(BuffZone.transform, false);
        Image newBuff = image.AddComponent<Image>();
        newBuff.sprite = buff.PackSprite;

        if (buff.CardName == ECardName.Appaloosa)
            Scope++;
        else if (buff.CardName == ECardName.Mustang)
            Position++;
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
        else if (buff.CardName == ECardName.Mustang)
            Position--;
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

    private void Death(Character enemy)
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

        _backRoleImage.gameObject.SetActive(false);
        IsDead = true;

        foreach (Character bot in UIElements.Instance.Enemies)
        {
            bot.botEnemies.Remove(this);
        }
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

    public void StartMove()
    {
        _imageOfDesk.color = new Color(0.8676471f, 0.7360041f, 0.0f);
        AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
        AddCardToHand(PackAndDiscard.Instance.GetRandomCard());

        if (Type == ECharacterType.Bot)
        {
            StartCoroutine(AIMove.StartMove());
            EndMove();
        }
    }

    public void EndMove()
    {
        if (Hand.Count > Health.Length && Type == ECharacterType.Player)
        {
            Actions.Instance.ShowPlayerCards();
            GlobalVeriables.GameState = EGameState.DropCards;
        }

        if (Type == ECharacterType.Bot && _usingCards.activeSelf)
        {
            foreach (Image card in _usingCards.GetComponentsInChildren<Image>())
                Destroy(card.gameObject);
        }

        if (Type == ECharacterType.Bot)
            _usingCards.SetActive(false);

        _imageOfDesk.color = Color.white;

        PlayersMoveQueue.StartNextPlayer();
    }

    public void UsingCard(PackAsset card)
    {
        _usingCards.SetActive(true);

        GameObject image = new GameObject();
        image.transform.SetParent(_usingCards.transform, false);
        Image newBuff = image.AddComponent<Image>();
        newBuff.sprite = card.PackSprite;
    }
}