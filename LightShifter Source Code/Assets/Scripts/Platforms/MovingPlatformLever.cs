using System;
using UnityEngine;

namespace Platforms
{
    public class MovingPlatformLever : MonoBehaviour
    {
        public static MovingPlatformLever Instance;
        private Transform _platform;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        private Boolean _platformActivated;

        private int _direction = 1;
        [SerializeField] private float speed = 0.5f;
        
        private void Awake()
        {
            Instance = this;
        }
        private void OnDrawGizmos()
        {
            if (_platform != null && startPoint != null && endPoint != null)
            {
                Gizmos.DrawLine(_platform.transform.position, startPoint.transform.position);
                Gizmos.DrawLine(_platform.transform.position, endPoint.transform.position);
            }
        }

        Vector2 CurrentMovementTarget()
        {
            if (_direction == 1)
            {
                return startPoint.position;
            }

            return endPoint.position;
        }

        private void Update()
        {
            if (_platformActivated)
            {
                Vector2 target = CurrentMovementTarget();
                _platform.position = Vector2.Lerp(_platform.position, target, speed * Time.deltaTime);

                float distance = (target - (Vector2)_platform.position).magnitude;
                if (distance < 0.5f)
                {
                    _direction *= -1;
                }
            }
        }

        public void StartMoving(Boolean activate, Transform platform)
        {
            _platform = platform;
            if (activate)
                _platformActivated = true;
            else
                _platformActivated = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(transform);
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(null);
            }
        }
    }
}