using UnityEngine;
using System; // Needed for Action<T>

public enum GamepadBrand
{
    Unknown,
    PlayStation,
    Xbox,
    Switch
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    /// <summary>
    /// True if the user is on keyboard/mouse; false if on a gamepad.
    /// </summary>
    public bool IsKeyboard { get; private set; } = true;

    /// <summary>
    /// The brand of the currently used gamepad (PlayStation, Xbox, Switch, or Unknown).
    /// </summary>
    public GamepadBrand CurrentBrand { get; private set; } = GamepadBrand.Unknown;

    /// <summary>
    /// An event that fires whenever the control scheme changes.
    /// (bool isKeyboard, GamepadBrand brand)
    /// </summary>
    public event Action<bool, GamepadBrand> OnDeviceChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Call this method whenever the user changes devices 
    /// (e.g., from keyboard/mouse to a gamepad, or vice versa).
    /// </summary>
    /// <param name="isKeyboard">True if keyboard/mouse, false if gamepad.</param>
    /// <param name="brand">Which gamepad brand is now active.</param>
    public void UpdateControlScheme(bool isKeyboard, GamepadBrand brand)
    {
        IsKeyboard = isKeyboard;
        CurrentBrand = brand;

        Debug.Log($"TutorialManager: Control scheme updated -> " +
                  $"IsKeyboard={IsKeyboard}, Brand={CurrentBrand}");

        // Fire the event, notifying any listeners that the device changed
        OnDeviceChanged?.Invoke(IsKeyboard, CurrentBrand);
    }
    
}