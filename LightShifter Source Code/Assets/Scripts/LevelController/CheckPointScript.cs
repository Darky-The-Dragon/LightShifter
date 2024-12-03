using System;
using Platforms;
using UnityEngine;

namespace LevelController
{
    public class CheckPointScript : MonoBehaviour
    {
        public GameObject checkPoint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                RespawnPlatform.Instance.UpdateCheckpoint(checkPoint);
            }
        }
    }
}