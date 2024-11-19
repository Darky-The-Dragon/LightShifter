using UnityEngine;

namespace Interactable
{
    public class LeverControllerBlocks : MonoBehaviour
    {
        public GameObject platform; 
        public float moveDistance = 5f; 
        public Transform player; 
        public float activationRange = 2f; 

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
