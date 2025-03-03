using System.Collections;
using LevelController;
using UnityEngine;

namespace Platforms
{
    public class RespawnPlatform : MonoBehaviour
    {
        public static RespawnPlatform Instance;
        [SerializeField] private Transform respawnPlatform;
        public GameObject lastCheckpoint;
        public Vector2 newLastCheckpoint;
        [SerializeField] private GameObject player;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            lastCheckpoint = null;
            newLastCheckpoint = player.transform.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine(WaitAndRespawn());
        }

        public void UpdateCheckpoint(GameObject checkpoint)
        {
            lastCheckpoint = checkpoint;
        }

        public void NewUpdateCheckpoint(Vector2 checkpoint)
        {
            newLastCheckpoint = checkpoint;
        }

        private IEnumerator WaitAndRespawn()
        {
            yield return new WaitForSeconds(0.5f);
            SceneController.Instance.Respawn(newLastCheckpoint);
        }
    }
}