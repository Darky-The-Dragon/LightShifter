using Unity.VisualScripting;
using UnityEngine;

namespace LightShift
{
    public class BlockShiftDetection : MonoBehaviour
    {
        [SerializeField] private LightShifter lightShift;
        [SerializeField] private float collider_xSize_in = 0.58f, collider_xSize_out = 1f;
        private BoxCollider2D _boxCollider2D;
        private GameObject currentColliderObject;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("BlockShift"))
            {
                currentColliderObject = other.gameObject;
                // Debug.Log("Player entered obstacle " + other.name);
                lightShift.BlockShift();
                _boxCollider2D.size = new Vector2(collider_xSize_out, _boxCollider2D.size.y);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {

            if (other.CompareTag("BlockShift") && other.gameObject.name == currentColliderObject.name)
            {
                // Debug.Log("Player exited obstacle " + other.name);
                lightShift.EnableShift();
                _boxCollider2D.size = new Vector2(collider_xSize_in, _boxCollider2D.size.y);
            }
        }

    }
}