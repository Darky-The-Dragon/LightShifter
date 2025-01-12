using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void Chapter1()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Chapter2()
        {
            SceneManager.LoadScene(2);
        }
    }
}