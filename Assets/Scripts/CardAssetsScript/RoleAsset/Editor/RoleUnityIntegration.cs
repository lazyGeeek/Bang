using UnityEditor;

static class RoleUnityIntegration
{
    [MenuItem("Assets/Create/RoleAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility2.CreateAsset<RoleAsset>();
    }
}
