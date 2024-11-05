using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float aheadDistance;
        [SerializeField] private float cameraSpeed;
        private float _lookAhead;

        private void Update()
        {
            transform.position = new Vector3(player.position.x + _lookAhead, player.position.y, transform.position.z);
            _lookAhead = Mathf.Lerp(_lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
        }
    }
}
