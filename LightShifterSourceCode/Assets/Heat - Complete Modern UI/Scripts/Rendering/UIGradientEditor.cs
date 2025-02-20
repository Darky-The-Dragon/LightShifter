#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(UIGradient))]
    public class UIGradientEditor : Editor
    {
        private int currentTab;
        private GUISkin customSkin;

        private void OnEnable()
        {
            if (EditorGUIUtility.isProSkin)
                customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin);
            else
                customSkin = HeatUIEditorHandler.GetLightEditor(customSkin);
        }

        public override void OnInspectorGUI()
        {
            var _effectGradient = serializedObject.FindProperty("_effectGradient");
            var _gradientType = serializedObject.FindProperty("_gradientType");
            var _offset = serializedObject.FindProperty("_offset");
            var _zoom = serializedObject.FindProperty("_zoom");
            var _modifyVertices = serializedObject.FindProperty("_modifyVertices");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
            HeatUIEditorHandler.DrawPropertyCW(_effectGradient, customSkin, "Gradient", 100);
            HeatUIEditorHandler.DrawPropertyCW(_gradientType, customSkin, "Type", 100);
            HeatUIEditorHandler.DrawPropertyCW(_offset, customSkin, "Offset", 100);
            HeatUIEditorHandler.DrawPropertyCW(_zoom, customSkin, "Zoom", 100);
            _modifyVertices.boolValue =
                HeatUIEditorHandler.DrawToggle(_modifyVertices.boolValue, customSkin, "Complex Gradient");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif