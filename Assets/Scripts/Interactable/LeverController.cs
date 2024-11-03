using UnityEngine;

namespace Interactable
{
    public class LeverController : MonoBehaviour
    {
        public GameObject platform; // Assign your platform here
        public float moveDistance = 5f; // Distance the platform moves
        public Transform player; // Reference to the player's Transform
        public float activationRange = 2f; // Maximum distance to activate the lever

        private bool _isLeverPulled;
        private Vector2 _initialPosition;

        void Start()
        {
            _initialPosition = platform.transform.position;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
            {
                ToggleLever();
            }
        }

        bool IsPlayerInRange()
        {
            return Vector2.Distance(transform.position, player.position) <= activationRange;
        }

        void ToggleLever()
        {
            _isLeverPulled = !_isLeverPulled;
            MovePlatform(_isLeverPulled);
        }

        void MovePlatform(bool move)
        {
            if (move)
            {
                platform.transform.position = _initialPosition + new Vector2(0,moveDistance);
            }
            else
            {
                platform.transform.position = _initialPosition;
            }
        }
    }
}