using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackAndDiscard : MonoBehaviour
{
    [SerializeField]
    private List<PackAsset> pack;

    public List<CharacterAsset> characters;
    public List<RoleAsset> roles;
    
    private List<PackAsset> discard;
    
    public static PackAndDiscard Instance;

    private void Awake()
    {
        Instance = this;
    }

    public CharacterAsset GetRandomCharacter()
    {
        CharacterAsset character = characters[Random.Range(0, characters.Count)];
        characters.Remove(character);
        return character;
    }

    public RoleAsset GetRandomRole()
    {
        RoleAsset role = roles[Random.Range(0, roles.Count)];
        roles.Remove(role);
        return role;
    }

    //Random select card
    public PackAsset GetRandomCard()
    {
        if (pack.Count == 0)
            RemixPack();
        PackAsset card = pack[Random.Range(0, pack.Count)];
        pack.Remove(card);
        return card;
    }

    public void Discard(PackAsset s)
    {
        discard.Add(s);
    }
    
    private void RemixPack()
    {
        pack.AddRange(discard);
        discard.Clear();
    }
}
