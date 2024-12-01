using System.Collections;
using LevelController;
using UnityEngine;

namespace Platforms
{
    public class RespawnPlatform : MonoBehaviour
    {
        [SerializeField] private Transform respawnPlatform;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine(WaitAndRespawn());
        }

        private IEnumerator WaitAndRespawn()
        {
            yield return new WaitForSeconds(0.5f);
            SceneController.Instance.Respawn();
        }
    }
}