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

        public void GoChapter2()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Assets/Scenes/GameScenes/Chapters/Chapter2.unity");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (SceneManager.GetActiveScene().path == "Assets/Scenes/GameScenes/Chapters/Chapter1.unity")
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Assets/Scenes/GameScenes/Chapters/Chapter2.unity");
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }
}