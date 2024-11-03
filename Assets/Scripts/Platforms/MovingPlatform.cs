using UnityEngine;

namespace Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform platform;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        private int _direction = 1;
        [SerializeField] private float speed = 0.5f;


        private void OnDrawGizmos()
        {
            if (platform != null && startPoint != null && endPoint != null)
            {
                Gizmos.DrawLine(platform.transform.position, startPoint.transform.position);
                Gizmos.DrawLine(platform.transform.position, endPoint.transform.position);
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
            Vector2 target = CurrentMovementTarget();
            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);
        
            float distance = (target -(Vector2)platform.position).magnitude;
            if (distance < 0.5f)
            {
                _direction *= -1;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.transform.SetParent(platform);
        }
    }
}