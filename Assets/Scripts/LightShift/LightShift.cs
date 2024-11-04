using UnityEngine;

namespace LightShift
{

    public class LightShift : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;       
        [SerializeField] private GameObject platform;     
        [SerializeField] private Color newBackgroundColor; 

        private Color _originalBackgroundColor;           
        private bool _isChanged;                           

        void Start()
        {
            // Cache the original background color and hide the platform initially
            if (mainCamera != null)
                _originalBackgroundColor = mainCamera.backgroundColor;

            if (platform != null)
                platform.SetActive(false); // Start with the platform hidden
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

            if (platform != null)
            {
                // Toggle platform visibility
                platform.SetActive(!_isChanged);
            }

            // Flip the toggle state
            _isChanged = !_isChanged;
        }
    }

}