using UnityEngine;

public enum ERole
{
    Assistant,
    Bandit,
    Renegade,
    Sheriff
}

public class RoleAsset : ScriptableObject
{
    public ERole Role;
    public Sprite RoleSpite;
}
