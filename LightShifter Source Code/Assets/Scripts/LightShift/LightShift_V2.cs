using System.Collections.Generic;
using UnityEngine;

namespace LightShift
{

    public class LightShift_V2 : MonoBehaviour
    {

        [SerializeField] private bool isLight = true;

        [SerializeField] private GameObject shiftingPlatform;

        private PlatformManager _lightPlatformManager;
        private PlatformManager _darkPlatformManager;

        private Sprite _lightBackground, _darkBackground;

        private SpriteRenderer _backgroundRenderer;

        void Awake()
        {
            _lightBackground = Resources.Load<Sprite>("TestImages/lightBackground");
            _darkBackground = Resources.Load<Sprite>("TestImages/darkBackground");
            _backgroundRenderer = FindObjectOfType<Background>().GetComponent<SpriteRenderer>();
            shiftingPlatform = FindObjectOfType<Platform>().gameObject;

            List<PlatformManager> tempPlatforms = new List<PlatformManager>(FindObjectsOfType<PlatformManager>());
            _lightPlatformManager = tempPlatforms.Find(x => x.getSide() == PlatformSide.LIGHT);
            _darkPlatformManager = tempPlatforms.Find(x => x.getSide() == PlatformSide.DARK);


            if (_backgroundRenderer == null || _lightBackground == null || _darkBackground == null || _lightPlatformManager == null || _darkPlatformManager == null)
            {
                Debug.LogError("Component not found");
            }

            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;

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
            _lightPlatformManager.SetPlatformsState(isLight);
            _darkPlatformManager.SetPlatformsState(!isLight);
        }
    }

}