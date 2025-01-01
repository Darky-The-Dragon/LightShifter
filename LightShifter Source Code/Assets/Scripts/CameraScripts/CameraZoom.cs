using Cinemachine;
using UnityEngine;

namespace CameraScripts
{
    public class CameraZoom : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;  // Reference to your Cinemachine Virtual Camera
        public Transform player;                        // Reference to the player
        public float zoomedInSize = 3f;                // Initial zoomed-in Orthographic size
        public float normalSize = 6f;                  // Normal Orthographic size when player is moving
        public float zoomSpeed = 5f;                   // Speed at which the camera zooms in/out

        private float currentSpeed;                    

        void Start()
        {
            // Set the camera's initial Orthographic Size to zoomed-in value
            virtualCamera.m_Lens.OrthographicSize = zoomedInSize;
        }

        void Update()
        {
            // Get the player's movement speed (assuming the player has a Rigidbody2D or similar)
            currentSpeed = player.GetComponent<Rigidbody2D>().velocity.magnitude;

            // Smoothly change the Orthographic Size based on the player's movement speed
            if (currentSpeed > 0.1f)  // If player is moving
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, normalSize, Time.deltaTime * zoomSpeed);
            }
            else  // If player is not moving
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomedInSize, Time.deltaTime * zoomSpeed);
            }
        }
    }
}
