using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelController
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance;
        [SerializeField] GameObject player;
        private Vector2 _startPosition;
        [SerializeField] Animator transitionAnim;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            _startPosition = player.transform.position;
        }

        private void Update()
        {
            Respawnevelopers();
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

        private void Respawnevelopers()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                player.transform.position = _startPosition;
            }
        }
        public void Respawn()
        {
            player.transform.position = _startPosition;
        }
    }
}