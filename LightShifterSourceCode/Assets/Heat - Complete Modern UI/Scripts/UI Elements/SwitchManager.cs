using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    public class SwitchManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        // Resources
        [SerializeField] private Animator switchAnimator;
        [SerializeField] private CanvasGroup highlightCG;

        // Saving
        public bool saveValue;
        public string saveKey = "My Switch";

        // Settings
        public bool isOn = true;
        public bool isInteractable = true;
        public bool invokeOnEnable = true;
        public bool useSounds = true;
        public bool useUINavigation;
        [Range(1, 15)] public float fadingMultiplier = 8;

        // Events
        [SerializeField] public SwitchEvent onValueChanged = new();
        public UnityEvent onEvents = new();
        public UnityEvent offEvents = new();

        private bool isInitialized;

        private void Awake()
        {
            if (saveValue)
            {
                GetSavedData();
            }
            else
            {
                if (gameObject.activeInHierarchy)
                {
                    StopCoroutine("DisableAnimator");
                    StartCoroutine("DisableAnimator");
                }

                switchAnimator.enabled = true;

                if (isOn)
                    switchAnimator.Play("On Instant");
                else
                    switchAnimator.Play("Off Instant");
            }

            if (gameObject.GetComponent<Image>() == null)
            {
                var raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }

            if (useUINavigation) AddUINavigation();
            if (highlightCG == null)
            {
                highlightCG = new GameObject().AddComponent<CanvasGroup>();
                highlightCG.transform.SetParent(transform);
                highlightCG.gameObject.name = "Highlighted";
            }

            if (invokeOnEnable && isOn)
            {
                onEvents.Invoke();
                onValueChanged.Invoke(isOn);
            }
            else if (invokeOnEnable && !isOn)
            {
                offEvents.Invoke();
                onValueChanged.Invoke(isOn);
            }

            isInitialized = true;
        }

        private void OnEnable()
        {
            if (UIManagerAudio.instance == null) useSounds = false;
            if (isInitialized) UpdateUI();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            StartCoroutine("SetNormal");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isInteractable || eventData.button != PointerEventData.InputButton.Left) return;
            if (useSounds)
                UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound);

            AnimateSwitch();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isInteractable) return;
            if (useSounds)
                UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound);

            StartCoroutine("SetHighlight");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isInteractable)
                return;

            StartCoroutine("SetNormal");
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            StartCoroutine("SetHighlight");
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            AnimateSwitch();
            StartCoroutine("SetNormal");
        }

        private void GetSavedData()
        {
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            switchAnimator.enabled = true;

            if (PlayerPrefs.GetString("Switch_" + saveKey) == "" || !PlayerPrefs.HasKey("Switch_" + saveKey))
            {
                if (isOn)
                {
                    switchAnimator.Play("Off");
                    PlayerPrefs.SetString("Switch_" + saveKey, "true");
                }
                else
                {
                    switchAnimator.Play("Off");
                    PlayerPrefs.SetString("Switch_" + saveKey, "false");
                }
            }
            else if (PlayerPrefs.GetString("Switch_" + saveKey) == "true")
            {
                switchAnimator.Play("Off");
                isOn = true;
            }
            else if (PlayerPrefs.GetString("Switch_" + saveKey) == "false")
            {
                switchAnimator.Play("Off");
                isOn = false;
            }
        }

        public void AddUINavigation()
        {
            var navButton = gameObject.AddComponent<Button>();
            navButton.transition = Selectable.Transition.None;
            var customNav = new Navigation();
            customNav.mode = Navigation.Mode.Automatic;
            navButton.navigation = customNav;
        }

        public void AnimateSwitch()
        {
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            switchAnimator.enabled = true;

            if (isOn)
            {
                switchAnimator.Play("Off");
                isOn = false;
                offEvents.Invoke();

                if (saveValue) PlayerPrefs.SetString("Switch_" + saveKey, "false");
            }

            else
            {
                switchAnimator.Play("On");
                isOn = true;
                onEvents.Invoke();

                if (saveValue) PlayerPrefs.SetString("Switch_" + saveKey, "true");
            }

            onValueChanged.Invoke(isOn);
        }

        public void SetOn()
        {
            if (saveValue) PlayerPrefs.SetString("Switch_" + saveKey, "true");
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            switchAnimator.enabled = true;
            switchAnimator.Play("On");

            isOn = true;
            onEvents.Invoke();
            onValueChanged.Invoke(true);
        }

        public void SetOff()
        {
            if (saveValue) PlayerPrefs.SetString("Switch_" + saveKey, "false");
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            switchAnimator.enabled = true;
            switchAnimator.Play("Off");

            isOn = false;
            offEvents.Invoke();
            onValueChanged.Invoke(false);
        }

        public void UpdateUI()
        {
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            switchAnimator.enabled = true;

            if (isOn && switchAnimator.gameObject.activeInHierarchy)
                switchAnimator.Play("On Instant");
            else if (!isOn && switchAnimator.gameObject.activeInHierarchy) switchAnimator.Play("Off Instant");
        }

        private IEnumerator DisableAnimator()
        {
            yield return new WaitForSeconds(0.5f);
            switchAnimator.enabled = false;
        }

        private IEnumerator SetNormal()
        {
            StopCoroutine("SetHighlight");

            while (highlightCG.alpha > 0.01f)
            {
                highlightCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            highlightCG.alpha = 0;
        }

        private IEnumerator SetHighlight()
        {
            StopCoroutine("SetNormal");

            while (highlightCG.alpha < 0.99f)
            {
                highlightCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            highlightCG.alpha = 1;
        }

        [Serializable]
        public class SwitchEvent : UnityEvent<bool>
        {
        }
    }
}