using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
                // write here the code to handle the touch of an enemy to the player
                // Time.timescale = 0 pauses the game, put it back to 1 to resume it
                Time.timeScale = 0;
        }
    }
}