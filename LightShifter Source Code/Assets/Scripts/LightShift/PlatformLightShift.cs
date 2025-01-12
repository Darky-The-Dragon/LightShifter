using LightShift;
using UnityEngine;
// For Action<T>

// If your LightShifter is in this namespace

public class PlatformLightShift : MonoBehaviour
{
    [Header("Child Objects Containing the SpriteRenderers")] [SerializeField]
    private GameObject lightPlatform;

    [SerializeField] private GameObject darkPlatform;

    [Header("Reference to LightShifter (optional)")] [SerializeField]
    private LightShifter lightShifter;

    private void Awake()
    {
        // If not assigned, try to find a LightShifter in the scene
        if (!lightShifter)
        {
            lightShifter = FindObjectOfType<LightShifter>();
            if (!lightShifter) Debug.LogError($"[{name}] No LightShifter found in the scene!");
        }
    }

    private void Start()
    {
        Debug.Log("Lighthift Status on Awake: " + lightShifter.IsLight);
        // Initialize based on the current mode in LightShifter
        if (lightShifter) SetActivePlatform(lightShifter.IsLight);
    }

    private void OnEnable()
    {
        // Subscribe to the event if LightShifter is available
        if (lightShifter) lightShifter.OnWorldShifted += HandleWorldShifted;
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled to avoid errors
        if (lightShifter) lightShifter.OnWorldShifted -= HandleWorldShifted;
    }

    /// <summary>
    ///     Called whenever LightShifter toggles environment (Light -> Dark or vice versa).
    /// </summary>
    private void HandleWorldShifted(bool isLight)
    {
        SetActivePlatform(isLight);
    }

    /// <summary>
    ///     Activates the LightPlatform or DarkPlatform child based on isLight mode.
    /// </summary>
    private void SetActivePlatform(bool isLight)
    {
        // If LightPlatform or DarkPlatform is missing, 
        // you could add safety checks or logs
        if (lightPlatform) lightPlatform.SetActive(isLight);
        if (darkPlatform) darkPlatform.SetActive(!isLight);
    }
}