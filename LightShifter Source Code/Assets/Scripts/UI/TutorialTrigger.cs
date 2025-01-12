using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TutorialTrigger : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private TutorialWindow tutorialWindow;

    [Header("Trigger Settings")] [SerializeField]
    private bool hideOnExit;

    [SerializeField] private bool triggerOnce = true;
    private int _collidersInside; // Tracks the number of player colliders inside the trigger

    private bool _triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _collidersInside++;

        if (_collidersInside > 1) return; // Prevent multiple triggers for the same player
        if (triggerOnce && _triggered) return;

        _triggered = true;

        // Initially show the window with the current device
        var isKeyboard = TutorialManager.Instance.IsKeyboard;
        var brand = TutorialManager.Instance.CurrentBrand;

        tutorialWindow.ShowForDevice(isKeyboard, brand);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _collidersInside--;

        // Only hide if no player colliders are inside
        if (_collidersInside <= 0)
        {
            _collidersInside = 0; // Reset to prevent negative counts
            if (hideOnExit) tutorialWindow.HideWindow();
        }
    }
}