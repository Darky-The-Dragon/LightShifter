using UnityEngine;

namespace Platforms
{
    public class MovingPlatformLever : MonoBehaviour
    {
        public static MovingPlatformLever Instance;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float speed = 0.5f;

        private int _direction = 1;
        private Transform _platform;
        private bool _platformActivated;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_platformActivated)
            {
                var target = CurrentMovementTarget();
                _platform.position = Vector2.Lerp(_platform.position, target, speed * Time.deltaTime);

                var distance = (target - (Vector2)_platform.position).magnitude;
                if (distance < 0.5f) _direction *= -1;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) collision.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) collision.transform.SetParent(null);
        }

        private void OnDrawGizmos()
        {
            if (_platform != null && startPoint != null && endPoint != null)
            {
                Gizmos.DrawLine(_platform.transform.position, startPoint.transform.position);
                Gizmos.DrawLine(_platform.transform.position, endPoint.transform.position);
            }
        }

        private Vector2 CurrentMovementTarget()
        {
            if (_direction == 1) return startPoint.position;

            return endPoint.position;
        }

        public void StartMoving(bool activate, Transform platform)
        {
            _platform = platform;
            if (activate)
                _platformActivated = true;
            else
                _platformActivated = false;
        }
    }
}