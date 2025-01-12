#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MenuManager))]
    public class MenuManagerEditor : Editor
    {
        private GUISkin customSkin;
        private MenuManager mmTarget;

        private void OnEnable()
        {
            mmTarget = (MenuManager)target;

            if (EditorGUIUtility.isProSkin)
                customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin);
            else
                customSkin = HeatUIEditorHandler.GetLightEditor(customSkin);
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var splashScreen = serializedObject.FindProperty("splashScreen");
            var mainContent = serializedObject.FindProperty("mainContent");
            var initPanel = serializedObject.FindProperty("initPanel");

            HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
            HeatUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            HeatUIEditorHandler.DrawProperty(splashScreen, customSkin, "Splash Screen");
            HeatUIEditorHandler.DrawProperty(mainContent, customSkin, "Main Content");
            HeatUIEditorHandler.DrawProperty(initPanel, customSkin, "Init Screen");

            if (mmTarget.UIManagerAsset != null)
            {
                HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 10);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                mmTarget.UIManagerAsset.enableSplashScreen = GUILayout.Toggle(
                    mmTarget.UIManagerAsset.enableSplashScreen, "Enable Splash Screen", customSkin.FindStyle("Toggle"));
                mmTarget.UIManagerAsset.enableSplashScreen = GUILayout.Toggle(
                    mmTarget.UIManagerAsset.enableSplashScreen, new GUIContent(""),
                    customSkin.FindStyle("Toggle Helper"));
                GUILayout.EndHorizontal();

                if (mmTarget.splashScreen != null)
                {
                    GUILayout.BeginHorizontal();

                    if (Application.isPlaying == false)
                    {
                        if (mmTarget.splashScreen.gameObject.activeSelf == false &&
                            GUILayout.Button("Show Splash Screen", customSkin.button))
                        {
                            mmTarget.splashScreen.gameObject.SetActive(true);
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        }

                        else if (mmTarget.splashScreen.gameObject.activeSelf &&
                                 GUILayout.Button("Hide Splash Screen", customSkin.button))
                        {
                            mmTarget.splashScreen.gameObject.SetActive(false);
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        }
                    }

                    if (GUILayout.Button("Select Splash Screen", customSkin.button))
                        Selection.activeObject = mmTarget.splashScreen;
                    GUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif