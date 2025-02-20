using LightShift;
using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private LightShifter lightShift;
        [SerializeField] private float collider_xSize_in = 0.58f, collider_xSize_out = 1f;
        private BoxCollider2D _boxCollider2D;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("BlockShift"))
            {
                Debug.Log("Player collided with obstacle");
                lightShift.BlockShift();
                _boxCollider2D.size = new Vector2(collider_xSize_out, _boxCollider2D.size.y);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("BlockShift"))
            {
                Debug.Log("Player left obstacle");
                lightShift.EnableShift();
                _boxCollider2D.size = new Vector2(collider_xSize_in, _boxCollider2D.size.y);
            }
        }
    }
}