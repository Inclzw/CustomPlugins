using UnityEditor;
using UnityEditor.SceneManagement;

namespace Main.Editor
{
    public static class Tools
    {
        // [MenuItem("Tools/Scene/Init", priority = 0)]
        // public static void OpenInitScene()
        // {
        //     EditorSceneManager.OpenScene("Assets/Main/Addressable/Scenes/Init.unity");
        // }

        [MenuItem("Tools/Reload Domain", priority = 1)]
        public static void ReloadDomain()
        {
            EditorUtility.RequestScriptReload();
        }
    }
}