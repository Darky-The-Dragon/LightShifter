using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class EndGame : MonoBehaviour
    {
        public GameObject pauseMenu;
        public Collider2D endLevelCollider;

        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) PauseGame();
        }
    }
}