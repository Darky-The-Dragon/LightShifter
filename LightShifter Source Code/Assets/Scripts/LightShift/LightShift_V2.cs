using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightShift
{
    public class LightShift_V2 : MonoBehaviour
    {
        [SerializeField] private bool isLight = true;
        public float shiftCooldownTime = 0.3f;

        private SpriteRenderer _backgroundRenderer;
        private PlatformManager _darkPlatformManager;

        private Sprite _lightBackground, _darkBackground;
        private PlatformManager _lightPlatformManager;
        private bool _shiftAvailable = true;

        private void Awake()
        {
            _lightBackground = Resources.Load<Sprite>("TestImages/lightBackground");
            _darkBackground = Resources.Load<Sprite>("TestImages/darkBackground");
            _backgroundRenderer = FindObjectOfType<Background>().GetComponent<SpriteRenderer>();

            if (_backgroundRenderer == null || _lightBackground == null || _darkBackground == null)
                Debug.LogError("Component not found");

            _backgroundRenderer.sprite = isLight ? _lightBackground : _darkBackground;
        }


        /*
            As the script needs to interact with other game objects, we must use Start to ensure they have been initialized
            and loaded in the scene before we try to access them.
            Start is run after Awake, but with the scene fully loaded and only if the game object is active!
        */
        private void Start()
        {
            var tempPlatforms = new List<PlatformManager>(FindObjectsOfType<PlatformManager>());
            _lightPlatformManager = tempPlatforms.Find(x => x.GetSide() == PlatformSide.LIGHT);
            _darkPlatformManager = tempPlatforms.Find(x => x.GetSide() == PlatformSide.DARK);

            if (_lightPlatformManager == null || _darkPlatformManager == null) Debug.LogError("Component not found");

            SetLightState(isLight);
        }

        private void Update()
        {
            // Check for 'Left Shift' key press once per frame
            if (Input.GetKeyDown(KeyCode.Q) && _shiftAvailable)
            {
                SetLightState(!isLight);
                StartCoroutine(ShiftCooldown());
            }
        }

        private IEnumerator ShiftCooldown()
        {
            _shiftAvailable = false;
            yield return new WaitForSeconds(shiftCooldownTime);
            _shiftAvailable = true;
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