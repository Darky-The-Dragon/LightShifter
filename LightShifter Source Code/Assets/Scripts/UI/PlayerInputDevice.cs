using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace UI
{
    public class PlayerInputDevice : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM
        private PlayerInput playerInput;

        private void Awake()
        {
            // (Optional) If the PlayerInput is on the same GameObject,
            // grab it here
            playerInput = GetComponent<PlayerInput>();
            
            if (playerInput == null)
            {
                Debug.LogWarning("No PlayerInput found in this scene!");
            }
        }

        private void OnEnable()
        {
            //Debug.Log("DeviceSchemeDetector Awake/OnEnable called!"); 
            if (playerInput != null)
                playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            if (playerInput != null)
                playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void OnControlsChanged(PlayerInput obj)
        {
            Debug.Log("Active scheme: " + obj.currentControlScheme);

            // 1. Check if it's keyboard/mouse or gamepad
            var isKeyboard = obj.currentControlScheme == "Keyboard&Mouse";

            // 2. If it's gamepad, detect the brand
            var brand = GamepadBrand.Unknown;
            if (!isKeyboard) brand = DetectGamepadBrand(Gamepad.current);

            // 3. Finally, notify the TutorialManager
            TutorialManager.Instance.UpdateControlScheme(isKeyboard, brand);
        }

        private GamepadBrand DetectGamepadBrand(Gamepad gp)
        {
            if (gp == null) return GamepadBrand.Unknown;

            var product = gp.description.product?.ToLower() ?? "";
            var manufacturer = gp.description.manufacturer?.ToLower() ?? "";

            Debug.Log("Product: " + product);
            Debug.Log("Manufacturer: " + manufacturer);


            if (product.Contains("sony") || product.Contains("dualshock") ||
                product.Contains("ps5") || product.Contains("ps4") ||
                product.Contains("dualsense") ||
                manufacturer.Contains("sony"))
                return GamepadBrand.PlayStation;

            if (product.Contains("xbox") || manufacturer.Contains("microsoft") ||
                product.Contains("xinput"))
                return GamepadBrand.Xbox;

            if (product.Contains("switch") || product.Contains("joy-con") ||
                product.Contains("nintendo") || manufacturer.Contains("nintendo"))
                return GamepadBrand.Switch;

            return GamepadBrand.Unknown;
        }
        #endif
    }
}
