#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Michsky.UI.Heat
{
    public class InitHeatUI
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("HeatUI.HasCustomEditorData"))
                {
                    var darkPath = AssetDatabase.GetAssetPath(Resources.Load("HeatUIEditor-Dark"));
                    var lightPath = AssetDatabase.GetAssetPath(Resources.Load("HeatUIEditor-Light"));

                    EditorPrefs.SetString("HeatUI.CustomEditorDark", darkPath);
                    EditorPrefs.SetString("HeatUI.CustomEditorLight", lightPath);
                    EditorPrefs.SetInt("HeatUI.HasCustomEditorData", 1);
                }
            }
        }
    }
}
#endif