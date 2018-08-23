using UnityEditor;

public class Pack
{
    [MenuItem("Assets/Create/PackAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility2.CreateAsset<PackAsset>();
    }
}
