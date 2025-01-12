using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelController
{
    public class EndLevelScript : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D endLevelCollider;
        [SerializeField] private GameObject player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}