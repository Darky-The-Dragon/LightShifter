using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Heat
{
    [CreateAssetMenu(fileName = "New Localization Settings",
        menuName = "Heat UI/Localization/New Localization Settings")]
    public class LocalizationSettings : ScriptableObject
    {
        // Global Variables
        public static string notInitializedText = "NOT_INITIALIZED";
        public List<Language> languages = new();
        public List<Table> tables = new();
        public string defaultLanguageID;
        public int defaultLanguageIndex;
        public bool enableExperimental;

        [Serializable]
        public class Language
        {
            public string languageID = "en-US";
            public string languageName = "English";
            public string localizedName = "English (US)";
            public LocalizationLanguage localizationLanguage;
#if UNITY_EDITOR
            [HideInInspector] public bool isExpanded;
#endif
        }

        [Serializable]
        public class Table
        {
            public string tableID;
            public LocalizationTable localizationTable;
#if UNITY_EDITOR
            [HideInInspector] public bool isExpanded;
#endif
        }
    }
}