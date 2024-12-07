using System;
using UnityEngine;

namespace LevelController
{
    public class ColliderControllerScript : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                LightShift.LightShift.CanChange = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                LightShift.LightShift.CanChange = true;
        }
    }
}
