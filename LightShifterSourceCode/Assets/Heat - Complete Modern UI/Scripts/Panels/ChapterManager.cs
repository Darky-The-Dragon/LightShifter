using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    public class ChapterManager : MonoBehaviour
    {
        public enum ChapterState
        {
            Locked,
            Unlocked,
            Completed,
            Current
        }

        // Content
        public List<ChapterItem> chapters = new();
        public int currentChapterIndex;

        // Resources
        [SerializeField] private GameObject chapterPreset;
        [SerializeField] private Transform chapterParent;
        [SerializeField] private ButtonManager previousButton;
        [SerializeField] private ButtonManager nextButton;
        [SerializeField] private Image progressFill;

        // Settings
        [SerializeField] private bool showLockedChapters = true;
        [SerializeField] private bool setPanelAuto = true;
        [SerializeField] private bool checkChapterData = true;
        [SerializeField] private bool useLocalization = true;
        [SerializeField] [Range(0.5f, 10)] private float barCurveSpeed = 2f;
        [SerializeField] private AnimationCurve barCurve = new(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [SerializeField] [Range(0.75f, 2)] private float animationSpeed = 1;

        // Background Animation
        [SerializeField] private bool backgroundStretch = true;
        [SerializeField] [Range(0, 100)] private float maxStretch = 75;
        [SerializeField] [Range(0.02f, 0.5f)] private float stretchCurveSpeed = 0.1f;
        [SerializeField] private AnimationCurve stretchCurve = new(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        public ChapterChangeCallback onChapterPanelChanged = new();

        // Helpers
        [HideInInspector] public RectTransform currentBackgroundRect;
        private readonly string animSpeedKey = "AnimSpeed";
        private readonly List<ChapterIdentifier> identifiers = new();
        private LocalizedObject localizedObject;

        private void Awake()
        {
            InitializeChapters();
        }

        private void OnEnable()
        {
            OpenCurrentPanel();
        }

        private void OnDisable()
        {
            if (backgroundStretch == false)
                return;

            StopCoroutine("StretchPhaseOne");
            StopCoroutine("StretchPhaseTwo");
        }

        public void DoStretch()
        {
            if (backgroundStretch == false || currentBackgroundRect == null || gameObject.activeInHierarchy == false)
                return;

            var calcSize = 1 + maxStretch / 960;
            currentBackgroundRect.localScale = new Vector3(calcSize, calcSize, calcSize);
            currentBackgroundRect.offsetMin = new Vector2(0, 0);
            currentBackgroundRect.offsetMax = new Vector2(0, 0);

            StopCoroutine("StretchPhaseOne");
            StopCoroutine("StretchPhaseTwo");
            StartCoroutine("StretchPhaseOne");
        }

        public void InitializeChapters()
        {
            if (useLocalization)
            {
                localizedObject = gameObject.GetComponent<LocalizedObject>();
                if (localizedObject == null || localizedObject.CheckLocalizationStatus() == false)
                    useLocalization = false;
            }

            identifiers.Clear();

            foreach (Transform child in chapterParent) Destroy(child.gameObject);
            for (var i = 0; i < chapters.Count; ++i)
            {
                var tempIndex = i;

                var itemGO = Instantiate(chapterPreset, new Vector3(0, 0, 0), Quaternion.identity);
                itemGO.transform.SetParent(chapterParent, false);
                itemGO.gameObject.name = chapters[i].chapterID;

                var item = itemGO.GetComponent<ChapterIdentifier>();
                item.chapterManager = this;
                identifiers.Add(item);

                // Background
                item.backgroundImage.sprite = chapters[i].background;
                item.UpdateBackgroundRect();

                // Title
                if (useLocalization == false || string.IsNullOrEmpty(chapters[i].titleKey))
                {
                    item.titleObject.text = chapters[i].title;
                }
                else
                {
                    var tempLoc = item.titleObject.GetComponent<LocalizedObject>();
                    if (tempLoc != null)
                    {
                        tempLoc.tableIndex = localizedObject.tableIndex;
                        tempLoc.localizationKey = chapters[i].titleKey;
                        tempLoc.UpdateItem();
                    }
                }

                // Description
                if (useLocalization == false || string.IsNullOrEmpty(chapters[i].descriptionKey))
                {
                    item.descriptionObject.text = chapters[i].description;
                }
                else
                {
                    var tempLoc = item.descriptionObject.GetComponent<LocalizedObject>();
                    if (tempLoc != null)
                    {
                        tempLoc.tableIndex = localizedObject.tableIndex;
                        tempLoc.localizationKey = chapters[i].descriptionKey;
                        tempLoc.UpdateItem();
                    }
                }

                // Set events
                item.continueButton.onClick.RemoveAllListeners();
                item.continueButton.onClick.AddListener(chapters[tempIndex].onContinue.Invoke);

                item.playButton.onClick.RemoveAllListeners();
                item.playButton.onClick.AddListener(chapters[tempIndex].onPlay.Invoke);

                item.replayButton.onClick.RemoveAllListeners();
                item.replayButton.onClick.AddListener(chapters[tempIndex].onReplay.Invoke);

                // Check for chapter data
                if (checkChapterData)
                {
                    if (!PlayerPrefs.HasKey("ChapterState_" + chapters[i].chapterID))
                    {
                        if (chapters[i].defaultState == ChapterState.Unlocked)
                            item.SetUnlocked();
                        else if (chapters[i].defaultState == ChapterState.Locked)
                            item.SetLocked();
                        else if (chapters[i].defaultState == ChapterState.Completed)
                            item.SetCompleted();
                        else
                            item.SetCurrent();
                    }
                    else if (PlayerPrefs.HasKey("ChapterState_" + chapters[i].chapterID) &&
                             PlayerPrefs.GetString("ChapterState_" + chapters[i].chapterID) == "unlocked")
                    {
                        item.SetUnlocked();
                    }
                    else if (PlayerPrefs.HasKey("ChapterState_" + chapters[i].chapterID) &&
                             PlayerPrefs.GetString("ChapterState_" + chapters[i].chapterID) == "current")
                    {
                        item.SetCurrent();
                    }
                    else if (PlayerPrefs.HasKey("ChapterState_" + chapters[i].chapterID) &&
                             PlayerPrefs.GetString("ChapterState_" + chapters[i].chapterID) == "completed")
                    {
                        item.SetCompleted();
                    }
                    else
                    {
                        item.SetLocked();
                    }
                }

                else
                {
                    if (chapters[i].defaultState == ChapterState.Unlocked)
                        item.SetUnlocked();
                    else if (chapters[i].defaultState == ChapterState.Locked)
                        item.SetLocked();
                    else if (chapters[i].defaultState == ChapterState.Completed)
                        item.SetCompleted();
                    else
                        item.SetCurrent();
                }

                // Set visibility
                itemGO.SetActive(false);
                if (setPanelAuto && item.isCurrent) currentChapterIndex = i;
            }

            // Set the current go active
            identifiers[currentChapterIndex].gameObject.SetActive(true);

            // Set button events
            previousButton.onClick.RemoveAllListeners();
            previousButton.onClick.AddListener(PrevChapter);

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextChapter);

            UpdateButtonState();

            if (progressFill != null)
                progressFill.fillAmount = 1f / chapters.Count * (currentChapterIndex + 1);
        }

        public void NextChapter()
        {
            if (currentChapterIndex + 1 > identifiers.Count - 1 ||
                (showLockedChapters == false && identifiers[currentChapterIndex + 1].isLocked))
                return;

            identifiers[currentChapterIndex].animator.enabled = true;
            identifiers[currentChapterIndex].animator.SetFloat(animSpeedKey, animationSpeed);
            identifiers[currentChapterIndex].animator.Play("Next Out");

            currentChapterIndex++;

            identifiers[currentChapterIndex].gameObject.SetActive(true);
            identifiers[currentChapterIndex].animator.enabled = true;
            identifiers[currentChapterIndex].animator.SetFloat(animSpeedKey, animationSpeed);
            identifiers[currentChapterIndex].animator.Play("Next In");
            identifiers[currentChapterIndex].UpdateBackgroundRect();

            UpdateButtonState();

            StopCoroutine("DisablePanels");
            StartCoroutine("DisablePanels");
        }

        public void PrevChapter()
        {
            if (currentChapterIndex <= 0 ||
                (showLockedChapters == false && identifiers[currentChapterIndex - 1].isLocked))
                return;

            identifiers[currentChapterIndex].animator.enabled = true;
            identifiers[currentChapterIndex].animator.SetFloat(animSpeedKey, animationSpeed);
            identifiers[currentChapterIndex].animator.Play("Prev Out");

            currentChapterIndex--;

            identifiers[currentChapterIndex].gameObject.SetActive(true);
            identifiers[currentChapterIndex].animator.enabled = true;
            identifiers[currentChapterIndex].animator.SetFloat(animSpeedKey, animationSpeed);
            identifiers[currentChapterIndex].animator.Play("Prev In");
            identifiers[currentChapterIndex].UpdateBackgroundRect();

            UpdateButtonState();

            StopCoroutine("DisablePanels");
            StartCoroutine("DisablePanels");
        }

        private void OpenCurrentPanel()
        {
            if (identifiers[currentChapterIndex] == null)
                return;

            identifiers[currentChapterIndex].gameObject.SetActive(true);
            identifiers[currentChapterIndex].animator.enabled = true;
            identifiers[currentChapterIndex].animator.SetFloat(animSpeedKey, animationSpeed);
            identifiers[currentChapterIndex].animator.Play("Start");
            identifiers[currentChapterIndex].UpdateBackgroundRect();
        }

        private void UpdateButtonState()
        {
            if (currentChapterIndex >= identifiers.Count - 1 && nextButton != null)
            {
                nextButton.isInteractable = false;
                nextButton.UpdateUI();
            }
            else if (nextButton != null)
            {
                nextButton.isInteractable = true;
                nextButton.UpdateUI();
            }

            if (currentChapterIndex < 1 && previousButton != null)
            {
                previousButton.isInteractable = false;
                previousButton.UpdateUI();
            }
            else if (previousButton != null)
            {
                previousButton.isInteractable = true;
                previousButton.UpdateUI();
            }

            if (showLockedChapters == false && currentChapterIndex <= identifiers.Count - 1 &&
                identifiers[currentChapterIndex + 1].isLocked)
            {
                nextButton.isInteractable = false;
                nextButton.UpdateUI();
            }

            if (progressFill != null && gameObject.activeInHierarchy)
            {
                StopCoroutine("PlayProgressFill");
                StartCoroutine("PlayProgressFill");
            }

            onChapterPanelChanged.Invoke(currentChapterIndex);
        }

        public static void SetLocked(string chapterID)
        {
            PlayerPrefs.SetString("ChapterState_" + chapterID, "locked");
        }

        public static void SetUnlocked(string chapterID)
        {
            PlayerPrefs.SetString("ChapterState_" + chapterID, "unlocked");
        }

        public static void SetCurrent(string chapterID)
        {
            PlayerPrefs.SetString("ChapterState_" + chapterID, "current");
        }

        public static void SetCompleted(string chapterID)
        {
            PlayerPrefs.SetString("ChapterState_" + chapterID, "completed");
        }

        private IEnumerator StretchPhaseOne()
        {
            float elapsedTime = 0;
            var startPos = currentBackgroundRect.offsetMin;
            var endPos = new Vector2(-maxStretch, 0);

            while (currentBackgroundRect.offsetMin.x > -maxStretch + 0.1f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                currentBackgroundRect.offsetMin = Vector2.Lerp(startPos, endPos,
                    stretchCurve.Evaluate(elapsedTime * stretchCurveSpeed));
                currentBackgroundRect.offsetMax = Vector2.Lerp(startPos, endPos,
                    stretchCurve.Evaluate(elapsedTime * stretchCurveSpeed));
                yield return null;
            }

            StartCoroutine("StretchPhaseTwo");
        }

        private IEnumerator StretchPhaseTwo()
        {
            float elapsedTime = 0;
            var startPos = currentBackgroundRect.offsetMin;
            var endPos = new Vector2(maxStretch, 0);

            while (currentBackgroundRect.offsetMin.x < maxStretch - 0.1f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                currentBackgroundRect.offsetMin = Vector2.Lerp(startPos, endPos,
                    stretchCurve.Evaluate(elapsedTime * stretchCurveSpeed));
                currentBackgroundRect.offsetMax = Vector2.Lerp(startPos, endPos,
                    stretchCurve.Evaluate(elapsedTime * stretchCurveSpeed));
                yield return null;
            }

            StartCoroutine("StretchPhaseOne");
        }

        private IEnumerator DisablePanels()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            for (var i = 0; i < identifiers.Count; i++)
            {
                identifiers[i].animator.enabled = false;

                if (i == currentChapterIndex)
                    continue;

                identifiers[i].gameObject.SetActive(false);
            }
        }

        private IEnumerator PlayProgressFill()
        {
            var startingPoint = progressFill.fillAmount;
            var dividedFill = 1f / chapters.Count;
            var toBeFilled = dividedFill * (currentChapterIndex + 1);
            float elapsedTime = 0;

            while (progressFill.fillAmount.ToString("F2") != toBeFilled.ToString("F2"))
            {
                elapsedTime += Time.unscaledDeltaTime;
                progressFill.fillAmount =
                    Mathf.Lerp(startingPoint, toBeFilled, barCurve.Evaluate(elapsedTime * barCurveSpeed));
                yield return null;
            }

            progressFill.fillAmount = toBeFilled;
        }

        // Events
        [Serializable]
        public class ChapterChangeCallback : UnityEvent<int>
        {
        }

        [Serializable]
        public class ChapterItem
        {
            public string chapterID;
            public string title;
            public Sprite background;
            [TextArea] public string description;
            public ChapterState defaultState;

            [Header("Localization")] public string titleKey = "TitleKey";

            public string descriptionKey = "DescriptionKey";

            [Header("Events")] public UnityEvent onContinue;

            public UnityEvent onPlay;
            public UnityEvent onReplay;
        }
    }
}