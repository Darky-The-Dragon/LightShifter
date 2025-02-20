using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class UIManagerSplashScreen : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private UIManager UIManagerAsset;

        [SerializeField] private bool mobileMode;

        [Header("Resources")] [SerializeField] private TextMeshProUGUI PAKStart;

        [SerializeField] private TextMeshProUGUI PAKKey;
        [SerializeField] private TextMeshProUGUI PAKEnd;

        private void Awake()
        {
            enabled = true;

            if (UIManagerAsset == null) UIManagerAsset = Resources.Load<UIManager>("Heat UI Manager");
            if (UIManagerAsset.enableDynamicUpdate == false)
            {
                UpdatePAK();
                enabled = false;
            }
        }

        private void Update()
        {
            if (UIManagerAsset == null) return;
            if (UIManagerAsset.enableDynamicUpdate) UpdatePAK();
            if (Application.isPlaying) enabled = false;
        }

        private void AnalyzePAKText()
        {
            if (mobileMode)
                return;

            // Fetch text and process formatting
            var tempText = UIManagerAsset.pakText;
            var outMain = tempText.Split(char.Parse("{"));
            var outStart = outMain[0];

            // Apply the first part if available
            if (!string.IsNullOrEmpty(outStart) && PAKStart != null)
            {
                PAKStart.gameObject.SetActive(true);
                PAKStart.text = outStart.Substring(0, outStart.Length - 1);
            }
            else if (PAKStart != null)
            {
                PAKStart.gameObject.SetActive(false);
            }

            // If there is no key text available, return
            if (outMain.Length <= 1)
            {
                if (PAKKey != null) PAKKey.transform.parent.gameObject.SetActive(false);
                if (PAKEnd != null) PAKEnd.gameObject.SetActive(false);
                return;
            }

            // Check for PAK text
            var outPak = outMain[1].Split(new[] { "}" }, StringSplitOptions.None);

            // Apply PAK Text part if available
            if (!string.IsNullOrEmpty(outPak[0]) && PAKKey != null)
            {
                PAKKey.transform.parent.gameObject.SetActive(true);
                PAKKey.text = outPak[0];
            }
            else if (PAKKey != null)
            {
                PAKKey.transform.parent.gameObject.SetActive(false);
            }

            // If there is no end text available, return
            if (outPak.Length <= 1)
            {
                if (PAKEnd != null) PAKEnd.gameObject.SetActive(false);
                return;
            }

            // Apply the last part if available
            if (!string.IsNullOrEmpty(outPak[1]) && PAKEnd != null)
            {
                PAKEnd.gameObject.SetActive(true);
                PAKEnd.text = outPak[1].Substring(1, outPak[1].Length - 1);
            }
            else if (PAKEnd != null)
            {
                PAKEnd.gameObject.SetActive(false);
            }
        }

        private void AnalyzePAKLocalizationText()
        {
            if (Application.isPlaying == false || mobileMode)
                return;

            var localStart = PAKStart.GetComponent<LocalizedObject>();
            var localKey = PAKKey.GetComponent<LocalizedObject>();
            var localEnd = PAKEnd.GetComponent<LocalizedObject>();

            if (localStart == null || localKey == null || localEnd == null)
                return;

            // Fetch text and process formatting
            var tempText = UIManagerAsset.pakLocalizationText;
            var outMain = tempText.Split(char.Parse("{"));
            var outStart = outMain[0];

            // Apply the first part if available
            if (!string.IsNullOrEmpty(outStart) && PAKStart != null)
            {
                outStart = outStart.Substring(0, outStart.Length - 1);
                outStart = localStart.GetKeyOutput(outStart);

                PAKStart.gameObject.SetActive(true);
                PAKStart.text = outStart;

                LayoutRebuilder.ForceRebuildLayoutImmediate(PAKStart.transform.parent.GetComponent<RectTransform>());
            }
            else if (PAKStart != null)
            {
                PAKStart.gameObject.SetActive(false);
            }

            // If there is no key text available, return
            if (outMain.Length <= 1)
            {
                if (PAKKey != null) PAKKey.transform.parent.gameObject.SetActive(false);
                if (PAKEnd != null) PAKEnd.gameObject.SetActive(false);
                return;
            }

            // Check for PAK text
            var outPak = outMain[1].Split(new[] { "}" }, StringSplitOptions.None);
            var outPakParsed = outPak[0];

            // Apply PAK Text part if available
            if (!string.IsNullOrEmpty(outPak[0]) && PAKKey != null)
            {
                outPakParsed = localKey.GetKeyOutput(outPakParsed);

                PAKKey.transform.parent.gameObject.SetActive(true);
                PAKKey.text = outPakParsed;

                LayoutRebuilder.ForceRebuildLayoutImmediate(PAKKey.transform.parent.GetComponent<RectTransform>());
            }
            else if (PAKKey != null)
            {
                PAKKey.transform.parent.gameObject.SetActive(false);
            }

            // If there is no end text available, return
            if (outPak.Length <= 1)
            {
                if (PAKEnd != null) PAKEnd.gameObject.SetActive(false);
                return;
            }

            // Apply the last part if available
            if (!string.IsNullOrEmpty(outPak[1]) && PAKEnd != null)
            {
                var outEndParsed = outPak[1].Substring(1, outPak[1].Length - 1);
                outEndParsed = localEnd.GetKeyOutput(outEndParsed);

                PAKEnd.gameObject.SetActive(true);
                PAKEnd.text = outEndParsed;

                LayoutRebuilder.ForceRebuildLayoutImmediate(PAKEnd.transform.parent.GetComponent<RectTransform>());
            }
            else if (PAKEnd != null)
            {
                PAKEnd.gameObject.SetActive(false);
            }
        }

        private void UpdatePAK()
        {
            if (UIManagerAsset.pakType == UIManager.PressAnyKeyTextType.Custom)
                return;

            if (UIManagerAsset.pakType == UIManager.PressAnyKeyTextType.Default &&
                UIManagerAsset.enableLocalization == false)
                AnalyzePAKText();
            else if (UIManagerAsset.pakType == UIManager.PressAnyKeyTextType.Default &&
                     UIManagerAsset.enableLocalization) AnalyzePAKLocalizationText();
        }
    }
}