using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelController
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance;
        [SerializeField] Animator transitionAnim;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void NextLevel()
        {
            StartCoroutine(LoadLevel());
        }

        IEnumerator LoadLevel()
        {
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            transitionAnim.SetTrigger("Start");
        }
    }
}