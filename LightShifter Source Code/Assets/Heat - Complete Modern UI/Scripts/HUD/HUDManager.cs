using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Heat
{
    public class HUDManager : MonoBehaviour
    {
        public enum DefaultBehaviour
        {
            Visible,
            Invisible
        }

        // Resources
        public GameObject HUDPanel;

        // Settings
        [Range(1, 20)] public float fadeSpeed = 8;
        public DefaultBehaviour defaultBehaviour = DefaultBehaviour.Visible;

        // Events
        public UnityEvent onSetVisible;
        public UnityEvent onSetInvisible;
        private CanvasGroup cg;

        // Helpers
        private bool isOn;

        private void Awake()
        {
            if (HUDPanel == null)
                return;

            cg = HUDPanel.AddComponent<CanvasGroup>();

            if (defaultBehaviour == DefaultBehaviour.Visible)
            {
                cg.alpha = 1;
                isOn = true;
                onSetVisible.Invoke();
            }
            else if (defaultBehaviour == DefaultBehaviour.Invisible)
            {
                cg.alpha = 0;
                isOn = false;
                onSetInvisible.Invoke();
            }
        }

        public void SetVisible()
        {
            if (isOn)
                SetVisible(false);
            else
                SetVisible(true);
        }

        public void SetVisible(bool value)
        {
            if (HUDPanel == null)
                return;

            if (value)
            {
                isOn = true;
                onSetVisible.Invoke();

                StopCoroutine("DoFadeIn");
                StopCoroutine("DoFadeOut");
                StartCoroutine("DoFadeIn");
            }

            else
            {
                isOn = false;
                onSetInvisible.Invoke();

                StopCoroutine("DoFadeIn");
                StopCoroutine("DoFadeOut");
                StartCoroutine("DoFadeOut");
            }
        }

        private IEnumerator DoFadeIn()
        {
            while (cg.alpha < 0.99f)
            {
                cg.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }

            cg.alpha = 1;
        }

        private IEnumerator DoFadeOut()
        {
            while (cg.alpha > 0.01f)
            {
                cg.alpha -= Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }

            cg.alpha = 0;
        }
    }
}