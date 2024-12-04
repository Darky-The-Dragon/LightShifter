using UnityEngine;

namespace LightShift
{
    public class LightShift : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private Color newBackgroundColor;
        [SerializeField] private bool useColoredBackground;
        [SerializeField] private GameObject gridLight;
        [SerializeField] private GameObject gridDark;
        [SerializeField] private GameObject grid;
        [SerializeField] private GameObject environment;
        private bool _isChanged;
        [SerializeField] GameObject lightBackground, darkBackground;

        private Color _originalBackgroundColor;

        private void Start()
        {
            // Cache the original background color and hide the platform initially
            if (mainCamera != null)
                _originalBackgroundColor = mainCamera.backgroundColor;
            ShowLight();
        }

        private void Update()
        {
            // Check for 'E' key press once per frame
            if (Input.GetKeyDown(KeyCode.LeftShift)) ToggleEnvironment();
        }

        private void ToggleEnvironment()
        {
            if (mainCamera != null && useColoredBackground)
                // Toggle between original and new background colors only if colored background is enabled
                mainCamera.backgroundColor = _isChanged ? _originalBackgroundColor : newBackgroundColor;
            if (_isChanged)
                ShowLight();
            else
                ShowDark();
            // Flip the toggle state
            _isChanged = !_isChanged;
        }

        private void ShowLight()
        {
            // toggle the backgrounds only if we're using the game objects and not camera colors
            if (!useColoredBackground)
            {
                darkBackground.SetActive(false);
                lightBackground.SetActive(true);
            }

            foreach (Transform child in environment.transform)
                if (child.gameObject != grid)
                {
                    if (child.gameObject == gridLight)
                        child.gameObject.SetActive(true);
                    else
                        child.gameObject.SetActive(false);
                }
        }

        private void ShowDark()
        {
            if (!useColoredBackground)
            {
                lightBackground.SetActive(false);
                darkBackground.SetActive(true);
            }

            foreach (Transform child in environment.transform)
                if (child.gameObject != grid)
                {
                    if (child.gameObject == gridDark)
                        child.gameObject.SetActive(true);
                    else
                        child.gameObject.SetActive(false);
                }
        }
    }
}