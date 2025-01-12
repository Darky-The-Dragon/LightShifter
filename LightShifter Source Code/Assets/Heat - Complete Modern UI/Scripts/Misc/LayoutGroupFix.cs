using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Heat UI/Layout/Layout Group Fix")]
    public class LayoutGroupFix : MonoBehaviour
    {
        public enum RebuildMethod
        {
            ForceRebuild,
            MarkRebuild
        }

        [SerializeField] private bool fixOnEnable = true;
        [SerializeField] private bool fixWithDelay = true;
        [SerializeField] private RebuildMethod rebuildMethod;
        private readonly float fixDelay = 0.025f;

        private void OnEnable()
        {
#if UNITY_EDITOR
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            if (Application.isPlaying == false) return;
#endif
            if (fixWithDelay == false && fixOnEnable && rebuildMethod == RebuildMethod.ForceRebuild)
                ForceRebuild();
            else if (fixWithDelay == false && fixOnEnable && rebuildMethod == RebuildMethod.MarkRebuild)
                MarkRebuild();
            else if (fixWithDelay) StartCoroutine(FixDelay());
        }

        public void FixLayout()
        {
            if (fixWithDelay == false && rebuildMethod == RebuildMethod.ForceRebuild)
                ForceRebuild();
            else if (fixWithDelay == false && rebuildMethod == RebuildMethod.MarkRebuild)
                MarkRebuild();
            else
                StartCoroutine(FixDelay());
        }

        private void ForceRebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private void MarkRebuild()
        {
            LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
        }

        private IEnumerator FixDelay()
        {
            yield return new WaitForSecondsRealtime(fixDelay);

            if (rebuildMethod == RebuildMethod.ForceRebuild)
                ForceRebuild();
            else if (rebuildMethod == RebuildMethod.MarkRebuild) MarkRebuild();
        }
    }
}