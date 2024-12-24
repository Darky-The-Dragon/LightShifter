using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace LightShift
{
    public class LightShift_BugTest : MonoBehaviour
    {
        [SerializeField] private GameObject gridLight, gridDark;
        private SortingGroup _darkSortingGroup;
        private CompositeCollider2D _lightTilemapsCollider, _darkTilemapsCollider;
        private Collider2D[] _lightColliders, _darkColliders;
        private bool _isChanged;
        public bool _canChange;
        [SerializeField] private KeyCode lightShiftKey = KeyCode.LeftShift;
        [SerializeField] private float shiftCooldown = 0.5f;

        private void Awake()
        {
            // _lightTilemapsCollider = gridLight.GetComponentInChildren<ColliderGroup>().gameObject.GetComponent<CompositeCollider2D>();
            _lightColliders = gridLight.GetComponentsInChildren<Collider2D>();
            _darkSortingGroup = gridDark.GetComponent<SortingGroup>();
            // _darkTilemapsCollider = gridDark.GetComponentInChildren<ColliderGroup>().gameObject.GetComponent<CompositeCollider2D>();
            _darkColliders = gridDark.GetComponentsInChildren<Collider2D>();

        }


        private void Start()
        {
            ShowLight();
            _canChange = true;
        }
        public void CanChange(bool canChange)
        {
            this._canChange = canChange;
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
            if (_isChanged)
                ShowLight();
            else
                ShowDark();
            // Flip the toggle state
            _isChanged = !_isChanged;
        }

        private void ShowLight()
        {
            // _lightTilemapsCollider.isTrigger = false;
            // _darkTilemapsCollider.isTrigger = true;
            SetColliderTrigger(_lightColliders, false);
            SetColliderTrigger(_darkColliders, true);

            _darkSortingGroup.sortingOrder = 0;
        }

        private void ShowDark()
        {
            // _darkTilemapsCollider.isTrigger = false;
            // _lightTilemapsCollider.isTrigger = true;
            SetColliderTrigger(_darkColliders, false);
            SetColliderTrigger(_lightColliders, true);

            // 2 is to ensure that the dark grid is always on top of the light grid
            // THE LIGHT GRID HAS A DEFAULT SORTING LAYER OF 1
            _darkSortingGroup.sortingOrder = 2;
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