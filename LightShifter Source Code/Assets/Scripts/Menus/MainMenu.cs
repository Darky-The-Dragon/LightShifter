using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void ToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Chapter1()
        {
            SceneManager.LoadScene(1);
        }

        public void Chapter2()
        {
            SceneManager.LoadScene(2);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}