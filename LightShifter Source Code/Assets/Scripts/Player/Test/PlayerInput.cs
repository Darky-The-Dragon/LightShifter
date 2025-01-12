using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TarodevController
{
    public class TarodevPlayerInput : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash, _shift;

        private void Awake()
        {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _dash = _actions.Player.Dash;
            _shift = _actions.Player.LightShift;

            // (Optional) If the PlayerInput is on the same GameObject,
            // grab it here
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _actions.Enable();

            // If you have a PlayerInput component, subscribe to onControlsChanged
            if (_playerInput != null) _playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            _actions.Disable();

            if (_playerInput != null) _playerInput.onControlsChanged -= OnControlsChanged;
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

        public FrameInput Gather()
        {
            return new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                Move = _move.ReadValue<Vector2>(),
                LightShift = _shift.WasPressedThisFrame()
            };
        }
#else
        public FrameInput Gather()
        {
            return new FrameInput
            {
                JumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C),
                DashDown = Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
        }
#endif
    }

    public struct FrameInput
    {
        public Vector2 Move;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool LightShift;
    }
}