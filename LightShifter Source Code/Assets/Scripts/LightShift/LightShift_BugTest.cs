using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace LightShift
{
    public class LightShift_BugTest : MonoBehaviour
    {
        [SerializeField] private GameObject gridLight, gridDark;
        private SortingGroup _lightSortingGroup, _darkSortingGroup;
        private Collider2D[] _lightColliders, _darkColliders;
        [SerializeField] private bool startWithLight = true;
        private bool _isLight;
        public bool _canChange = true;
        [SerializeField] private KeyCode lightShiftKey = KeyCode.LeftShift;
        [SerializeField] private float shiftCooldown = 0.5f;

        private void Awake()
        {
            _lightSortingGroup = gridLight.GetComponent<SortingGroup>();
            _lightColliders = gridLight.GetComponentsInChildren<Collider2D>();

            _darkSortingGroup = gridDark.GetComponent<SortingGroup>();
            _darkColliders = gridDark.GetComponentsInChildren<Collider2D>();
        }


        private void Start()
        {
            if (startWithLight)
            {
                _isLight = true;
                ShowLight();
            }
            else
            {
                _isLight = false;
                ShowDark();
            }
            _canChange = true;
        }
        public void BlockShift()
        {
            this._canChange = false;
        }
        public void EnableShift()
        {
            this._canChange = true;
        }

        private IEnumerator ShiftCooldown()
        {
            _canChange = false;
            yield return new WaitForSeconds(shiftCooldown);
            _canChange = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(lightShiftKey) && _canChange)
            {
                ToggleEnvironment();
                StartCoroutine(ShiftCooldown());
            }
        }

        private void ToggleEnvironment()
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
            Debug.Log("Showing Light");
            SetColliderTrigger(_lightColliders, false);
            SetColliderTrigger(_darkColliders, true);

            // move the dark world beneath the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder - 1;
        }

        private void ShowDark()
        {
            Debug.Log("Showing Dark");

            SetColliderTrigger(_darkColliders, false);
            SetColliderTrigger(_lightColliders, true);

            // move the dark world on top of the light world
            _darkSortingGroup.sortingOrder = _lightSortingGroup.sortingOrder + 1;

        }

        private void SetColliderTrigger(Collider2D[] colliders, bool isTrigger)
        {
            foreach (Collider2D collider in colliders)
            {
                collider.isTrigger = isTrigger;
            }
        }


    }

}