using Platforms;
using UnityEngine;

namespace Interactable
{
    public class LeverController : MonoBehaviour
    {
        public Transform player; 
        public float activationRange = 2f;
        private bool _activated = true;
        [SerializeField] Transform platform;

        private bool _isLeverPulled;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
            {
                MovingPlatformLever.Instance.StartMoving(_activated, platform);
                _activated = !_activated;
            }
        }

        bool IsPlayerInRange()
        {
            return Vector2.Distance(transform.position, player.position) <= activationRange;
        }
    }
}