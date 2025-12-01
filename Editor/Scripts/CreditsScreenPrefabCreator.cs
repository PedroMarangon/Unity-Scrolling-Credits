using UnityEditor;
using UnityEngine;

namespace ScrollingCreditsScreen.Editor
{
    public class CreditsScreenPrefabCreator
    {
        private const string RESOURCE_FOLDER = "Packages/com.pedrommarangon.scrollingcredits/Assets/Resources/Credits/ScrollingCreditsCanvas.prefab";

        [MenuItem("GameObject/UI/Scrolling Credits", isValidateFunction: true, priority = -1)]
        public static bool Create()
        {
            return Selection.activeGameObject == null || (Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInParent<Canvas>() == null);
        }

        [MenuItem("GameObject/UI/Scrolling Credits", isValidateFunction: false, priority = -1)]
        public static void Create(MenuCommand menuCommand)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(RESOURCE_FOLDER);

            if (prefab == null)
            {
                Debug.LogError("ScrollingCreditsCanvas.prefab does not exist in the specified path.");
                return;
            }

            GameObject parent = Selection.activeGameObject;

            if(menuCommand.context as GameObject != null) parent = menuCommand.context as GameObject;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (parent != null) GameObjectUtility.SetParentAndAlign(instance, parent);

            Undo.RegisterCreatedObjectUndo(instance, "Created ScrollingCredits Prefab");
            Selection.activeGameObject = instance;
        }
	}
}
