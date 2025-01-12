using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    public class AchievementItem : MonoBehaviour
    {
        [Header("Default Resources")] public Image iconObj;

        public Image backgroundObj;
        public TextMeshProUGUI titleObj;
        public TextMeshProUGUI descriptionObj;
        public GameObject lockedIndicator;
        public GameObject unlockedIndicator;
        public List<Image> images = new();
    }
}