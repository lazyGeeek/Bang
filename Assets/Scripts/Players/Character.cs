using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public enum ECharacterType
{
    Player,
    Bot
}

public class Character : MonoBehaviour
{
    //Zone for displaying all buffs
    [SerializeField]
    private GameObject buffZone;

    [SerializeField]
    private Image characterImage;

    //If player get to jail activate this object
    [SerializeField]
    private Image jailImage;

    [SerializeField]
    private Image roleImage;

    [SerializeField]
    private Image backRoleImage;

    [SerializeField]
    private Image weaponImage;

    [SerializeField]
    private Image defaultWeaponImage;

    [SerializeField]
    private CanvasGroup bulletHole;

    [SerializeField]
    private Image[] health;
    
    //Enemy position relative to the player
    [SerializeField]
    private int position;
    public int Position
    {
        get { return position; }
        set { position = value; }
    }

    [SerializeField]
    private ECharacterType type;

    private List<PackAsset> hand = new List<PackAsset>();
    public List<PackAsset> Hand
    {
        get { return hand; }
    }

    private List<PackAsset> buffs = new List<PackAsset>();
    public List<PackAsset> Buffs
    {
        get { return buffs; }
    }

    public List<PackAsset> UsedCard = new List<PackAsset>(); //Used cards per turn

    private CharacterAsset characterInfo;
    public CharacterAsset CharacterInfo
    {
        get { return characterInfo; }
    }

    private RoleAsset roleInfo;
    public RoleAsset RoleInfo
    {
        get { return roleInfo; }
    }

    private int maxHealth;
    
    public bool IsDead { get; set; }
    public int  Scope { get; set; } //Scope of current gun

    private PackAsset weapon;
    public PackAsset Weapon
    {
        get { return weapon; }
        set
        {
            if (value == null)
                defaultWeaponImage.gameObject.SetActive(true);
            else
            {
                weaponImage.sprite = value.PackSprite;
                defaultWeaponImage.gameObject.SetActive(false);
            }
        }
    }
    
    private void Start()
    {
        characterInfo = PackAndDiscard.Instance.GetRandomCharacter();
        characterImage.sprite = characterInfo.CharacterSprite;
        maxHealth = characterInfo.MaxHealth;

        roleInfo = PackAndDiscard.Instance.GetRandomRole();
        roleImage.sprite = roleInfo.RoleSpite;
        if (roleInfo.Role == ERole.Sheriff)
            maxHealth++;
        if (roleInfo.Role == ERole.Sheriff || type == ECharacterType.Player)
            backRoleImage.gameObject.SetActive(false);

        Scope = 1;
        IsDead = false;

        SetHealth();
        
        if (characterInfo.Name == ECharacterName.PaulRegred)
            Position++;
        else if (characterInfo.Name == ECharacterName.RoseDoolan)
            Scope++;
    }

    private void SetHealth()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            health[i].gameObject.SetActive(true);
            AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
        }
    }

    public void AddCardToHand(PackAsset card)
    {
        hand.Add(card);
    }

    public void RemoveCardFromHand(PackAsset card)
    {
        hand.Remove(card);
    }

    public void RemoveCardToDiscard(PackAsset card)
    {
        hand.Remove(card);
        PackAndDiscard.Instance.Discard(card);
    }

    public void AddBuff(PackAsset buff)
    {
        Buffs.Add(buff);
        UsedCard.Add(buff);
        
        GameObject image = new GameObject();
        image.AddComponent<Image>();
        Image newBuff = Instantiate(image.GetComponent<Image>(), buffZone.transform);
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

        foreach (Image buffImage in buffZone.GetComponentsInChildren<Image>())
        {
            if (buffImage.sprite == buff.PackSprite)
                Destroy(buffImage);
        }

        if (buff.CardName == ECardName.Appaloosa)
            Scope--;
        else if (buff.CardName == ECardName.Mustang)
            Position--;
    }

    public void Hit(Character enemy = null) //TODO Complete this
    {
        if (!health[1].IsActive() && health[0].IsActive())
        {
            health[0].gameObject.SetActive(false);
            IsDead = true;

            backRoleImage.gameObject.SetActive(false);

            //Complete for death

            return;
        }

        for (int i = maxHealth - 1; i >= 0; --i)
        {
            if (health[i].IsActive())
            {
                health[i].gameObject.SetActive(false);
                break;
            }
            else if (!health[0].IsActive())
                Debug.Log("He already dead");
        }
        if (enemy != null)
            CheckCharacterAbilityForHit(enemy);
    }

    private void CheckCharacterAbilityForHit(Character enemy) //TODO Complete this
    {
        if (CharacterInfo.Name == ECharacterName.BartCassidy)
        {
            if (type == ECharacterType.Player)
            {
                ShowCards.Instance.ShowCardSpawn();

                PackAsset randomCard = PackAndDiscard.Instance.GetRandomCard();
                Image card = ShowCards.Instance.cardSpawn.AddComponent<Image>();
                card.sprite = randomCard.PackSprite;
                AddCardToHand(randomCard);
            }
            else
                AddCardToHand(PackAndDiscard.Instance.GetRandomCard());
        }
        else if (CharacterInfo.Name == ECharacterName.ElGringo)
        {
            if (type == ECharacterType.Player)
            {
                ShowCards.Instance.ShowCardSpawn();


            }
            else
            {
                PackAsset randomCard = enemy.Hand[Random.Range(0, enemy.Hand.Count)];
                enemy.RemoveCardFromHand(randomCard);
                Hand.Add(randomCard);
            }
        }
    }

    public bool Heal()
    {
        if (!health[maxHealth - 1].IsActive())
        {
            foreach (Image h in health)
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
        bulletHole.alpha = 1f;

        yield return new WaitForSeconds(1f);

        while (bulletHole.alpha > 0)
        {
            bulletHole.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void PutInJail()
    {
        jailImage.gameObject.SetActive(true);
    }

    public void FreeFromJail()
    {
        jailImage.gameObject.SetActive(false);
    }
}


