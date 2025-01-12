using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    [Header("Keyboard Objects")] [SerializeField]
    private GameObject[] keyboardObjects;

    [Header("PlayStation Objects")] [SerializeField]
    private GameObject[] playStationObjects;

    [Header("Xbox Objects")] [SerializeField]
    private GameObject[] xboxObjects;

    [Header("Switch Objects")] [SerializeField]
    private GameObject[] switchObjects;

    private void Awake()
    {
        // Optionally hide at startup
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Subscribe to device changes when this window is enabled
        if (TutorialManager.Instance != null) TutorialManager.Instance.OnDeviceChanged += HandleDeviceChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe when this window is disabled
        if (TutorialManager.Instance != null) TutorialManager.Instance.OnDeviceChanged -= HandleDeviceChanged;
    }

    /// <summary>
    ///     This method is called whenever the device changes
    ///     (via the event in TutorialManager).
    /// </summary>
    /// <param name="isKeyboard">True if keyboard, false if gamepad.</param>
    /// <param name="brand">The new gamepad brand.</param>
    private void HandleDeviceChanged(bool isKeyboard, GamepadBrand brand)
    {
        // If this window is currently active, update the displayed icons
        if (gameObject.activeSelf) ShowForDevice(isKeyboard, brand);
    }

    public void ShowForDevice(bool isKeyboard, GamepadBrand brand)
    {
        // Hide every brand’s array of objects
        DisableArray(keyboardObjects);
        DisableArray(playStationObjects);
        DisableArray(xboxObjects);
        DisableArray(switchObjects);

        // Enable the appropriate array
        if (isKeyboard)
            EnableArray(keyboardObjects);
        else
            switch (brand)
            {
                case GamepadBrand.PlayStation:
                    EnableArray(playStationObjects);
                    break;
                case GamepadBrand.Xbox:
                    EnableArray(xboxObjects);
                    break;
                case GamepadBrand.Switch:
                    EnableArray(switchObjects);
                    break;
            }

        // show the entire window
        gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    private void EnableArray(GameObject[] objects)
    {
        if (objects == null) return;
        foreach (var obj in objects)
            if (obj)
                obj.SetActive(true);
    }

    private void DisableArray(GameObject[] objects)
    {
        if (objects == null) return;
        foreach (var obj in objects)
            if (obj)
                obj.SetActive(false);
    }
}