using UnityEngine;
namespace LightShift
{

    public class LightShift : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private Color newBackgroundColor;
        
        [SerializeField] private GameObject gridLight;
        [SerializeField] private GameObject gridDark;
        [SerializeField] private GameObject grid;
        [SerializeField] private GameObject environment;
        
        private Color _originalBackgroundColor;           
        private bool _isChanged;                           

        void Start()
        {
            // Cache the original background color and hide the platform initially
            if (mainCamera != null)
                _originalBackgroundColor = mainCamera.backgroundColor;
            ShowLight();
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
            if (_isChanged)
            {
                ShowLight();
            }
            else
            {
                ShowDark();
            }
            // Flip the toggle state
            _isChanged = !_isChanged;
        }
        private void ShowLight()
        {
            foreach (Transform child in environment.transform)
            {
                if (child.gameObject != grid)
                {
                    if (child.gameObject == gridLight)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        private void ShowDark()
        {
            foreach (Transform child in environment.transform)
            {
                if (child.gameObject != grid)
                {
                    if (child.gameObject == gridDark)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

}