using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace LightShift
{
    public class LightShifter : MonoBehaviour
    {
        [SerializeField] private GameObject gridLight, gridDark;
        private SortingGroup _lightSortingGroup, _darkSortingGroup;
        private Collider2D[] _lightColliders, _darkColliders;
        [SerializeField] private bool startWithLight = true;
        private bool _isLight;
        [SerializeField] private bool canChange = true, isShiftLoaded = true;
        [SerializeField] private KeyCode lightShiftKey = KeyCode.LeftShift;
        [SerializeField] private float shiftCooldown = 0.5f;
        private LightShifter _instance;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip transitionClip;
        private float _volume = 0.1f;
        private bool _start;

        private void Awake()
        {
            if(_instance == null) {
                _instance = this;

                _lightSortingGroup = gridLight.GetComponent<SortingGroup>();
                _lightColliders = gridLight.GetComponentsInChildren<Collider2D>();

                _darkSortingGroup = gridDark.GetComponent<SortingGroup>();
                _darkColliders = gridDark.GetComponentsInChildren<Collider2D>();
            }
            else
                Destroy(gameObject);
            
        }


        private void Start()
        {
            _start = true;
            if (startWithLight)
            {
                _isLight = true;
                ShowLight();
                _start = false;
            }
            else
            {
                _isLight = false;
                ShowDark();
                _start = false;
            }
            canChange = true;
            isShiftLoaded = true;
        }
        public void BlockShift()
        {
            canChange = false;
        }
        public void EnableShift()
        {
            canChange = true;
        }

        private IEnumerator ShiftCooldown()
        {
            isShiftLoaded = false;
            yield return new WaitForSeconds(shiftCooldown);
            isShiftLoaded = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(lightShiftKey) && canChange && isShiftLoaded)
            {
                ToggleEnvironment();
                StartCoroutine(ShiftCooldown());
            }
        }
        
        public void ToggleEnvironment()
        {
            if (_isLight)
                ShowDark();
            else
                ShowLight();
            // Flip the toggle state
            _isLight = !_isLight;
        }

        private void ShowLight()
        {
            // Debug.Log("Showing Light");
            SetColliderTrigger(_lightColliders, false);
            SetColliderTrigger(_darkColliders, true);

            // move the dark world beneath the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder - 1;
            
            if(!_start)
                audioSource.PlayOneShot(transitionClip, _volume);
        }

        private void ShowDark()
        {
            // Debug.Log("Showing Dark");

            SetColliderTrigger(_darkColliders, false);
            SetColliderTrigger(_lightColliders, true);

            // move the dark world on top of the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder + 1;
            
            if(!_start)
                audioSource.PlayOneShot(transitionClip, _volume);

        }

        private void SetColliderTrigger(Collider2D[] colliders, bool isTrigger)
        {
            foreach (Collider2D collision in colliders)
            {
                collision.isTrigger = isTrigger;
            }
        }


    }

}