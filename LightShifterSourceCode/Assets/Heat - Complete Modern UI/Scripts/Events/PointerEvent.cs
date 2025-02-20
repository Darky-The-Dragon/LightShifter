using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.Heat
{
    public class PointerEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")] public bool addEventTrigger = true;

        [Header("Events")] [SerializeField] private UnityEvent onClick = new();

        [SerializeField] private UnityEvent onEnter = new();
        [SerializeField] private UnityEvent onExit = new();

        private void Awake()
        {
            if (addEventTrigger) gameObject.AddComponent<EventTrigger>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke();
        }
    }
}