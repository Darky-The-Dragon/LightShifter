using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Michsky.UI.Heat
{
    public class UISway : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public enum InputType
        {
            Mouse,
            Touchscreen
        }

        [Header("Resources")] [SerializeField] private Canvas mainCanvas;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private RectTransform swayObject;

        [Header("Settings")] [SerializeField] [Range(1, 20)]
        private float smoothness = 10;

        [SerializeField] private InputType inputType = InputType.Mouse;

        private bool allowSway;
        private Vector3 cursorPos;
        private Vector2 defaultPos;

        private void Start()
        {
            defaultPos = swayObject.anchoredPosition;

            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCanvas == null) mainCanvas = GetComponentInParent<Canvas>();
            if (swayObject == null) swayObject = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (allowSway && inputType == InputType.Mouse)
                cursorPos = Mouse.current.position.ReadValue();
            else if (allowSway && inputType == InputType.Touchscreen)
                cursorPos = Touchscreen.current.position.ReadValue();

            if (mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                ProcessOverlay();
            else if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera) ProcessSSC();
        }

        public void OnPointerEnter(PointerEventData data)
        {
            allowSway = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            allowSway = false;
        }

        private void ProcessOverlay()
        {
            if (allowSway)
                swayObject.position = Vector2.Lerp(swayObject.position, cursorPos, Time.unscaledDeltaTime * smoothness);
            else
                swayObject.localPosition = Vector2.Lerp(swayObject.localPosition, defaultPos,
                    Time.unscaledDeltaTime * smoothness);
        }

        private void ProcessSSC()
        {
            if (allowSway)
                swayObject.position = Vector2.Lerp(swayObject.position, mainCamera.ScreenToWorldPoint(cursorPos),
                    Time.unscaledDeltaTime * smoothness);
            else
                swayObject.localPosition = Vector2.Lerp(swayObject.localPosition, defaultPos,
                    Time.unscaledDeltaTime * smoothness);
        }
    }
}