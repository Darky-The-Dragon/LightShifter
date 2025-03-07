using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Heat
{
    [CreateAssetMenu(fileName = "New Controller Preset", menuName = "Heat UI/Controller/New Controller Preset")]
    public class ControllerPreset : ScriptableObject
    {
        public enum ItemType
        {
            Icon,
            Text
        }

        [Header("Settings")] public string controllerName = "Controller Name";

        [Space(10)] public List<ControllerItem> items = new();

        [Serializable]
        public class ControllerItem
        {
            public string itemID;
            public ItemType itemType;
            public Sprite itemIcon;
            public string itemText;
        }
    }
}