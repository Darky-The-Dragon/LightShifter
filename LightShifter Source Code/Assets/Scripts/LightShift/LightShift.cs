using UnityEngine;
namespace LightShift
{

    public class LightShift : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private Color newBackgroundColor; 

        private Color _originalBackgroundColor;           
        private bool _isChanged;                           

        void Start()
        {
            // Cache the original background color and hide the platform initially
            if (mainCamera != null)
                _originalBackgroundColor = mainCamera.backgroundColor;
        }

        void Update()
        {
            // Check for 'E' key press once per frame
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ToggleEnvironment();
            }
        }

        private void ToggleEnvironment()
        {
            if (mainCamera != null)
            {
                // Toggle between original and new background colors
                mainCamera.backgroundColor = _isChanged ? _originalBackgroundColor : newBackgroundColor;
            }
            // Flip the toggle state
            _isChanged = !_isChanged;
        }
    }

}