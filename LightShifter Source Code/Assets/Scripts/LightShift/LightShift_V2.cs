using System.Collections.Generic;
using UnityEngine;

namespace LightShift
{

    public class LightShift_V2 : MonoBehaviour
    {

        [SerializeField] private bool isLight = true;
        private PlatformManager _lightPlatformManager;
        private PlatformManager _darkPlatformManager;

        private Sprite _lightBackground, _darkBackground;

        private SpriteRenderer _backgroundRenderer;

        void Awake()
        {
            _lightBackground = Resources.Load<Sprite>("TestImages/lightBackground");
            _darkBackground = Resources.Load<Sprite>("TestImages/darkBackground");
            _backgroundRenderer = FindObjectOfType<Background>().GetComponent<SpriteRenderer>();

            if (_backgroundRenderer == null || _lightBackground == null || _darkBackground == null)
            {
                Debug.LogError("Component not found");
            }

            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;

        }


        /*
            As the script needs to interact with other game objects, we must use Start to ensure they have been initialized
            and loaded in the scene before we try to access them. 
            Start is run after Awake, but with the scene fully loaded and only if the game object is active!
        */
        void Start()
        {
            List<PlatformManager> tempPlatforms = new List<PlatformManager>(FindObjectsOfType<PlatformManager>());
            _lightPlatformManager = tempPlatforms.Find(x => x.GetSide() == PlatformSide.LIGHT);
            _darkPlatformManager = tempPlatforms.Find(x => x.GetSide() == PlatformSide.DARK);

            if (_lightPlatformManager == null || _darkPlatformManager == null)
            {
                Debug.LogError("Component not found");
            }

            SetLightState(isLight);
        }

        void Update()
        {
            // Check for 'Left Shift' key press once per frame
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetLightState(!isLight);
            }
        }

        private void SetLightState(bool newState)
        {
            print("Setting light state to " + newState);
            isLight = newState;
            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;
            // if (_lightPlatformManager == null || _darkPlatformManager == null)
            // {
            //     Debug.LogError("PlatformManager not found");
            // }
            _lightPlatformManager.SetPlatformsState(isLight);
            _darkPlatformManager.SetPlatformsState(!isLight);
        }
    }

}