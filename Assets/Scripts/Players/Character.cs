using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ECharacterType
{
    Player,
    Bot
}

public class Character : MonoBehaviour
{
    [SerializeField]
    private Image imageOfDesk;
    //If player get to jail activate this object
    [SerializeField]
    private Image jailImage;
    public Image JailImage
    {
        get { return jailImage; }
    }

    [SerializeField]
    private Image roleImage;

    [SerializeField]
    private Image backRoleImage;
    public Image BackRoleImage
    {
        get { return backRoleImage; }
    }

    [SerializeField]
    private Image weaponImage;

    [SerializeField]
    private Image defaultWeaponImage;

    [SerializeField]
    private CanvasGroup bulletHole;
    public CanvasGroup BulletHole
    {
        get { return bulletHole; }
    }

    [SerializeField]
    private Image[] _health;
    public Image[] Health
    {
        get { return _health; }
    }

    //Enemy position relative to the player
    [SerializeField]
    private int position;
    public int Position
    {
        get { return position; }
        set { position = value; }
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

    public List<PackAsset> UsedCard = new List<PackAsset>(); //Used cards per turn

    private RoleAsset roleInfo;
    public RoleAsset RoleInfo
    {
        get { return roleInfo; }
    }

    private int maxHealth = 4;
    
    public bool IsDead { get; set; }
    public bool InJail { get; set; }
    public int  Scope { get; set; } //Scope of current gun
    public Image CharacterImage;
    public GameObject BuffZone; //Zone for displaying all buffs

    private PackAsset weapon;
    public PackAsset Weapon
    {
        get { return weapon; }
        set
        {
            if (value == null)
            {
                Scope -= Actions.GetScope(weapon);
                defaultWeaponImage.gameObject.SetActive(true);
                weapon = null;
            }
            else
            {
                Scope -= Actions.GetScope(weapon);
                weaponImage.sprite = value.PackSprite;
                Scope += Actions.GetScope(value);
                defaultWeaponImage.gameObject.SetActive(false);
            }
        }
    }
    
    public static bool operator ==(Character ch1, Character ch2)
    {
        return ch1 != null && ch2 != null && ch1.CharacterImage.sprite == ch2.CharacterImage.sprite &&
               ch1.roleImage.sprite == ch2.roleImage.sprite &&
               ch1.position == ch2.position;
    }

    public static bool operator !=(Character ch1, Character ch2)
    {
        return !(ch1 == ch2);
    }

    public void InitiateCharacter()
    {
        CharacterImage.sprite = PackAndDiscard.Instance.GetRandomCharacter();
        roleInfo = PackAndDiscard.Instance.GetRandomRole();
        roleImage.sprite = roleInfo.RoleSpite;

        if (CharacterImage.sprite.name.Contains("el_gringo") || CharacterImage.sprite.name.Contains("paul_regred"))
            maxHealth--;

        if (roleInfo.Role == ERole.Sheriff)
            maxHealth++;
        if (roleInfo.Role == ERole.Sheriff || _type == ECharacterType.Player)
            backRoleImage.gameObject.SetActive(false);

        Scope = 1;
        IsDead = false;

        SetHealth();
    }

    private void SetHealth()
    {
        for (int i = 0; i < maxHealth; i++)
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
        UsedCard.Add(buff);

        GameObject image = new GameObject();
        image.AddComponent<Image>();
        Image newBuff = Instantiate(image.GetComponent<Image>(), BuffZone.transform);
        newBuff.sprite = buff.PackSprite;
        Destroy(image);

        if (buff.CardName == ECardName.Appaloosa)
            Scope++;
        else if (buff.CardName == ECardName.Mustang)
            Position++;
    }

    public void RemoveBuff(PackAsset buff)
    {
        Buffs.Remove(buff);
        PackAndDiscard.Instance.Discard(buff);

        foreach (Image buffImage in BuffZone.GetComponentsInChildren<Image>())
        {
            if (buffImage.sprite == buff.PackSprite)
                Destroy(buffImage);
        }

        if (buff.CardName == ECardName.Appaloosa)
            Scope--;
        else if (buff.CardName == ECardName.Mustang)
            Position--;
    }

    public void Hit(Character enemy = null)
    {
        if (!Health[1].IsActive() && Health[0].IsActive())
        {
            Health[0].gameObject.SetActive(false);
            IsDead = true;

            BackRoleImage.gameObject.SetActive(false);
            Death(enemy);

            return;
        }

        for (int i = maxHealth - 1; i >= 0; --i)
        {
            if (Health[i].IsActive())
            {
                Health[i].gameObject.SetActive(false);
                break;
            }
            else if (!Health[0].IsActive())
                Debug.Log("He is already dead");
        }
    }

    private void Death(Character enemy)
    {
        List<PackAsset> cards = new List<PackAsset>();

        cards.AddRange(Hand);
        Hand.Clear();
        cards.AddRange(Buffs);
        Buffs.Clear();
        if (Weapon != null)
            cards.Add(Weapon);
        Weapon = null;

        foreach (PackAsset card in cards)
        {
            if (enemy != null && enemy.RoleInfo.Role == ERole.Sheriff && RoleInfo.Role == ERole.Assistant)
                enemy.AddCardToHand(card);
            else
                PackAndDiscard.Instance.Discard(card);
        }

        backRoleImage.gameObject.SetActive(false);
        IsDead = true;
        FreeFromJail();
    }

    public bool Heal()
    {
        if (!Health[maxHealth - 1].IsActive())
        {
            foreach (Image h in Health)
            {
                if (!h.IsActive())
                {
                    h.gameObject.SetActive(true);
                    return true;
                }
            }
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

    public void PutInJail()
    {
        JailImage.gameObject.SetActive(true);
        InJail = true;
    }

    public void FreeFromJail()
    {
        JailImage.gameObject.SetActive(false);
        InJail = false;
    }

    public void StartMove()
    {
        imageOfDesk.color = new Color(0.8676471f, 0.7360041f, 0.0f);
        AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
        AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
    }

    public bool EndMove()
    {
        if (Hand.Count > Health.Length) //TODO show field for drop after fail "if"
            return false;
        imageOfDesk.color = Color.white;
        return true;
    }
}


