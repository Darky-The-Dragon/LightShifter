#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Michsky.UI.Heat
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GradientFilter))]
    public class GradientFilterEditor : Editor
    {
        private GUISkin customSkin;
        private GradientFilter gfTarget;

        private void OnEnable()
        {
            gfTarget = (GradientFilter)target;

            if (EditorGUIUtility.isProSkin)
                customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin);
            else
                customSkin = HeatUIEditorHandler.GetLightEditor(customSkin);
        }

        public override void OnInspectorGUI()
        {
            var selectedFilter = serializedObject.FindProperty("selectedFilter");
            var opacity = serializedObject.FindProperty("opacity");

            HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
            HeatUIEditorHandler.DrawProperty(selectedFilter, customSkin, "Selected Filter");
            HeatUIEditorHandler.DrawProperty(opacity, customSkin, "Opacity");

            gfTarget.UpdateFilter();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif