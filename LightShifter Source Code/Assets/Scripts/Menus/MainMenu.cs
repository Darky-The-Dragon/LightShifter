using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void ToMenu()
        {
            SceneManager.LoadScene("Assets/Scenes/MechanicsTest/UItest.unity");
        }

        public void StoryIntro()
        {
            SceneManager.LoadScene("Assets/Scenes/GameScenes/Chapters/StoryIntro.unity");
        }

        public void Chapter1()
        {
            SceneManager.LoadScene("Assets/Scenes/GameScenes/Chapters/Chapter1.unity");
        }

        public void Chapter2()
        {
            SceneManager.LoadScene("Assets/Scenes/GameScenes/Chapters/Chapter2.unity");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}