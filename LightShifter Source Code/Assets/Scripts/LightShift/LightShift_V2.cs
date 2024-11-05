using UnityEngine;

namespace LightShift
{

    public class LightShift_V2 : MonoBehaviour
    {

        [SerializeField] private bool isLight = true;

        [SerializeField] private GameObject shiftingPlatform;

        private Sprite _lightBackground, _darkBackground;

        private SpriteRenderer _backgroundRenderer;

        void Awake() {
            _lightBackground = Resources.Load<Sprite>("TestImages/lightBackground");
            _darkBackground = Resources.Load<Sprite>("TestImages/darkBackground");
            _backgroundRenderer = FindObjectOfType<Background>().GetComponent<SpriteRenderer>();
            shiftingPlatform = FindObjectOfType<Platform>().gameObject;


            if(_backgroundRenderer == null || _lightBackground == null || _darkBackground == null || shiftingPlatform == null) {
                Debug.LogError("Component not found");
            }

            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;
 
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
            isLight = !isLight;
            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;
            shiftingPlatform.SetActive(isLight);
        }
    }

}