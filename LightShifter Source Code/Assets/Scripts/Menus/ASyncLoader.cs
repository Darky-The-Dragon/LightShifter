using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus
{
    public class ASyncLoader : MonoBehaviour
    {
        [Header("Menu Screens")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject mainMenu;

        [Header("Slider")] 
        [SerializeField] private Slider loadingSlider;

        public void LoadLevelBtn(string levelToLoad)
        {
            mainMenu.SetActive(false);
            loadingScreen.SetActive(true);

            StartCoroutine(LoadLevelASync(levelToLoad));
        }

        private IEnumerator LoadLevelASync(string levelToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (loadOperation is { isDone: false })
            {
                var progressValue = loadOperation.progress;
                loadingSlider.value = progressValue;
                yield return null;
            }
        }
    }
}
