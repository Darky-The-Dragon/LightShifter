using Platforms;
using UnityEngine;

namespace Interactable
{
    public class LeverController : MonoBehaviour
    {
        public Transform player;
        public float activationRange = 2f;
        [SerializeField] private Transform platform;
        private bool _activated = true;

        private bool _isLeverPulled;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
            {
                MovingPlatformLever.Instance.StartMoving(_activated, platform);
                _activated = !_activated;
            }
        }

        private bool IsPlayerInRange()
        {
            return Vector2.Distance(transform.position, player.position) <= activationRange;
        }
    }
}