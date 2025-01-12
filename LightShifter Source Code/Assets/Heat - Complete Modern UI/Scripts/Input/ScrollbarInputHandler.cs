using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Michsky.UI.Heat
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollbarInputHandler : MonoBehaviour
    {
        public enum ScrollbarDirection
        {
            Horizontal,
            Vertical
        }

        [Header("Resources")] [SerializeField] private Scrollbar scrollbarObject;

        [SerializeField] private GameObject indicator;

        [Header("Settings")] [SerializeField] private ScrollbarDirection direction = ScrollbarDirection.Vertical;

        [Range(0.1f, 50)] public float scrollSpeed = 3;
        [SerializeField] [Range(0.01f, 1)] private float deadzone = 0.1f;
        [SerializeField] private bool optimizeUpdates = true;
        public bool allowInputs = true;
        [SerializeField] private bool reversePosition;

        // Helpers
        private readonly float divideValue = 1000;

        private void Update()
        {
            if (Gamepad.current == null || ControllerManager.instance == null || !allowInputs)
            {
                indicator.SetActive(false);
                return;
            }

            if (optimizeUpdates && ControllerManager.instance != null && !ControllerManager.instance.gamepadEnabled)
            {
                indicator.SetActive(false);
                return;
            }

            indicator.SetActive(true);

            if (direction == ScrollbarDirection.Vertical)
            {
                if (reversePosition && ControllerManager.instance.vAxis >= 0.1f)
                    scrollbarObject.value -= scrollSpeed / divideValue * ControllerManager.instance.vAxis;
                else if (!reversePosition && ControllerManager.instance.vAxis >= 0.1f)
                    scrollbarObject.value += scrollSpeed / divideValue * ControllerManager.instance.vAxis;
                else if (reversePosition && ControllerManager.instance.vAxis <= -0.1f)
                    scrollbarObject.value += scrollSpeed / divideValue * Mathf.Abs(ControllerManager.instance.vAxis);
                else if (!reversePosition && ControllerManager.instance.vAxis <= -0.1f)
                    scrollbarObject.value -= scrollSpeed / divideValue * Mathf.Abs(ControllerManager.instance.vAxis);
            }

            else if (direction == ScrollbarDirection.Horizontal)
            {
                if (reversePosition && ControllerManager.instance.hAxis >= deadzone)
                    scrollbarObject.value -= scrollSpeed / divideValue * ControllerManager.instance.hAxis;
                else if (!reversePosition && ControllerManager.instance.hAxis >= deadzone)
                    scrollbarObject.value += scrollSpeed / divideValue * ControllerManager.instance.hAxis;
                else if (reversePosition && ControllerManager.instance.hAxis <= -deadzone)
                    scrollbarObject.value += scrollSpeed / divideValue * Mathf.Abs(ControllerManager.instance.hAxis);
                else if (!reversePosition && ControllerManager.instance.hAxis <= -deadzone)
                    scrollbarObject.value -= scrollSpeed / divideValue * Mathf.Abs(ControllerManager.instance.hAxis);
            }
        }

        private void OnEnable()
        {
            if (scrollbarObject == null) scrollbarObject = gameObject.GetComponent<Scrollbar>();
            if (ControllerManager.instance == null) Destroy(this);
            if (indicator == null)
            {
                indicator = new GameObject();
                indicator.name = "[Generated Indicator]";
                indicator.transform.SetParent(transform);
            }

            indicator.SetActive(false);
        }
    }
}