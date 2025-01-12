using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    [RequireComponent(typeof(Slider))]
    public class SliderManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler,
        IDeselectHandler
    {
        // Resources
        public Slider mainSlider;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private CanvasGroup highlightCG;

        // Saving
        public bool saveValue;
        public bool invokeOnAwake = true;
        public string saveKey = "My Slider";

        // Settings
        public bool isInteractable = true;
        public bool usePercent;
        public bool showValue = true;
        public bool showPopupValue = true;
        public bool useRoundValue;
        public bool useSounds = true;
        [Range(1, 15)] public float fadingMultiplier = 8;
        [SerializeField] public SliderEvent onValueChanged = new();

        private void Awake()
        {
            if (highlightCG == null)
            {
                highlightCG = new GameObject().AddComponent<CanvasGroup>();
                highlightCG.gameObject.AddComponent<RectTransform>();
                highlightCG.transform.SetParent(transform);
                highlightCG.gameObject.name = "Highlight";
            }

            if (mainSlider == null) mainSlider = gameObject.GetComponent<Slider>();
            if (gameObject.GetComponent<Image>() == null)
            {
                var raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }

            highlightCG.alpha = 0;
            highlightCG.gameObject.SetActive(false);
            var saveVal = mainSlider.value;

            if (saveValue)
            {
                if (PlayerPrefs.HasKey("Slider_" + saveKey))
                    saveVal = PlayerPrefs.GetFloat("Slider_" + saveKey);
                else
                    PlayerPrefs.SetFloat("Slider_" + saveKey, saveVal);

                mainSlider.value = saveVal;
                mainSlider.onValueChanged.AddListener(delegate
                {
                    PlayerPrefs.SetFloat("Slider_" + saveKey, mainSlider.value);
                });
            }

            mainSlider.onValueChanged.AddListener(delegate
            {
                onValueChanged.Invoke(mainSlider.value);
                UpdateUI();
            });

            if (invokeOnAwake) onValueChanged.Invoke(mainSlider.value);
            UpdateUI();
        }

        private void Start()
        {
            if (UIManagerAudio.instance == null) useSounds = false;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (isInteractable == false)
                return;

            StartCoroutine("SetNormal");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (useSounds)
                UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound);
            if (isInteractable == false) return;

            StartCoroutine("SetHighlight");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isInteractable == false)
                return;

            StartCoroutine("SetNormal");
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (isInteractable == false)
                return;

            StartCoroutine("SetHighlight");
        }

        public void Interactable(bool value)
        {
            isInteractable = value;
            mainSlider.interactable = isInteractable;
        }

        public void AddUINavigation()
        {
            var customNav = new Navigation();
            customNav.mode = Navigation.Mode.Automatic;
            mainSlider.navigation = customNav;
        }

        public void UpdateUI()
        {
            if (valueText == null)
                return;

            if (useRoundValue)
            {
                if (usePercent && valueText != null)
                    valueText.text = Mathf.Round(mainSlider.value * 1.0f) + "%";
                else if (valueText != null) valueText.text = Mathf.Round(mainSlider.value * 1.0f).ToString();
            }

            else
            {
                if (usePercent && valueText != null)
                    valueText.text = mainSlider.value.ToString("F1") + "%";
                else if (valueText != null) valueText.text = mainSlider.value.ToString("F1");
            }
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
            highlightCG.gameObject.SetActive(false);
        }

        private IEnumerator SetHighlight()
        {
            StopCoroutine("SetNormal");
            highlightCG.gameObject.SetActive(true);

            while (highlightCG.alpha < 0.99f)
            {
                highlightCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            highlightCG.alpha = 1;
        }

        // Events
        [Serializable]
        public class SliderEvent : UnityEvent<float>
        {
        }
    }
}