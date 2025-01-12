using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Heat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class NavigationBar : MonoBehaviour
    {
        public enum BarDirection
        {
            Top,
            Bottom
        }

        public enum UpdateMode
        {
            DeltaTime,
            UnscaledTime
        }

        // Resources
        [SerializeField] private Animator animator;
        [SerializeField] private CanvasGroup canvasGroup;

        // Settings
        [SerializeField] private UpdateMode updateMode = UpdateMode.DeltaTime;
        [SerializeField] private BarDirection barDirection = BarDirection.Top;
        [SerializeField] private bool fadeButtons;
        [SerializeField] private Transform buttonParent;
        private readonly List<PanelButton> buttons = new();

        // Helpers
        private float cachedStateLength = 0.4f;

        private void Awake()
        {
            if (animator == null) GetComponent<Animator>();
            if (canvasGroup == null) GetComponent<CanvasGroup>();
            if (fadeButtons && buttonParent != null) FetchButtons();

            cachedStateLength = HeatUIInternalTools.GetAnimatorClipLength(animator, "NavigationBar_TopShow") + 0.02f;
        }

        private void OnEnable()
        {
            Show();
        }

        public void FetchButtons()
        {
            buttons.Clear();

            foreach (Transform child in buttonParent)
                if (child.GetComponent<PanelButton>() != null)
                {
                    var btn = child.GetComponent<PanelButton>();
                    btn.navbar = this;
                    buttons.Add(btn);
                }
        }

        public void LitButtons(PanelButton source = null)
        {
            foreach (var btn in buttons)
            {
                if (btn.isSelected || (source != null && btn == source))
                    continue;

                btn.IsInteractable(true);
            }
        }

        public void DimButtons(PanelButton source)
        {
            foreach (var btn in buttons)
            {
                if (btn.isSelected || btn == source)
                    continue;

                btn.IsInteractable(false);
            }
        }

        public void Show()
        {
            animator.enabled = true;

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");

            if (barDirection == BarDirection.Top)
                animator.Play("Top Show");
            else if (barDirection == BarDirection.Bottom) animator.Play("Bottom Show");
        }

        public void Hide()
        {
            animator.enabled = true;

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");

            if (barDirection == BarDirection.Top)
                animator.Play("Top Hide");
            else if (barDirection == BarDirection.Bottom) animator.Play("Bottom Hide");
        }

        private IEnumerator DisableAnimator()
        {
            if (updateMode == UpdateMode.DeltaTime)
                yield return new WaitForSeconds(cachedStateLength);
            else
                yield return new WaitForSecondsRealtime(cachedStateLength);

            animator.enabled = false;
        }
    }
}