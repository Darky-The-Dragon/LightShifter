using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace LightShift
{
    public class LightShift_BugTest : MonoBehaviour
    {
        [SerializeField] private GameObject gridLight, gridDark;
        [SerializeField] GameObject lightBackground, darkBackground;
        private SortingGroup _lightSortingGroup, _darkSortingGroup;
        private CompositeCollider2D _lightTilemapsCollider, _darkTilemapsCollider;
        private bool _isChanged;
        public bool _canChange;
        [SerializeField] private KeyCode lightShiftKey = KeyCode.LeftShift;


        private void Awake()
        {
            _lightSortingGroup = gridLight.GetComponent<SortingGroup>();
            _lightTilemapsCollider = gridLight.GetComponentInChildren<CompositeCollider2D>();

            _darkSortingGroup = gridDark.GetComponent<SortingGroup>();
            _darkTilemapsCollider = gridDark.GetComponentInChildren<CompositeCollider2D>();
        }


        private void Start()
        {
            ShowDark();
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

        private void Update()
        {
            if (Input.GetKeyDown(lightShiftKey) && _canChange)
            {
                ToggleEnvironment();
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
            _lightTilemapsCollider.isTrigger = false;
            _darkTilemapsCollider.isTrigger = true;

            _darkTilemapsCollider.enabled = false;
            // this is to ensure the collider is able to detect any collision 
            _darkTilemapsCollider.enabled = true;
            _darkSortingGroup.sortingOrder = 0;
        }

        private void ShowDark()
        {
            // lightBackground.SetActive(false);
            // darkBackground.SetActive(true);
            _darkTilemapsCollider.isTrigger = false;
            _lightTilemapsCollider.isTrigger = true;

            // this is to ensure the collider is able to detect any collision
            _lightTilemapsCollider.enabled = false;
            _lightTilemapsCollider.enabled = true;
            // 2 is to ensure that the dark grid is always on top of the light grid
            // THE LIGHT GRID HAS A DEFAULT SORTING LAYER OF 1
            _darkSortingGroup.sortingOrder = 2;
        }
    }

}