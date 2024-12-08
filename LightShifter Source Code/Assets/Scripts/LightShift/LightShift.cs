using UnityEngine;
using UnityEngine.Tilemaps;

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

        public bool _canChange;

        [SerializeField] private GameObject center;
        private Color _originalBackgroundColor;

        private Vector3Int _playerTilePosition;

        private int _playerX;
        private int _playerY;

        public static LightShift Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void CanChange(bool canChange)
        {
            this._canChange = canChange;
        }

        private void Start()
        {
            // Cache the original background color and hide the platform initially
            if (mainCamera != null)
                _originalBackgroundColor = mainCamera.backgroundColor;
            ShowLight();
            _canChange = true;
        }

        private void Update()
        {
            // Check for 'E' key press once per frame
            if (Input.GetKeyDown(KeyCode.LeftShift) && _canChange)
            {
                ToggleEnvironment();
            }
        }

        private void ToggleEnvironment()
        {
            // _playerX = Mathf.RoundToInt(center.transform.position.x);
            // _playerY = Mathf.RoundToInt(center.transform.position.y);
            // Vector3Int playerTilePosition = new Vector3Int(_playerX, _playerY, 0);
            // if (CheckCollisions(playerTilePosition))
            //     return;
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

        // CheckCollision to solve LightShift bug
        [SerializeField] GameObject player;


        [SerializeField] private Tilemap lightTilemap;
        [SerializeField] private Tilemap darkTilemap;

        private bool CheckCollisions(Vector3Int playerTilePosition)
        {
            Tilemap lightTileMap = gridLight.GetComponentInChildren<Tilemap>(true);

            if (lightTileMap.HasTile(playerTilePosition))
                return true;
            return false;
        }
    }

}