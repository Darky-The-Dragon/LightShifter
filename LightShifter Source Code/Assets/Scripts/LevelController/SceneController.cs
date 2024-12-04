using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelController
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance;
        [SerializeField] private GameObject player;
        [SerializeField] private Animator transitionAnim;
        private Vector2 _startPosition;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
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

        private IEnumerator LoadLevel()
        {
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            transitionAnim.SetTrigger("Start");
        }

        private void Respawnevelopers()
        {

            if (Input.GetKeyDown(KeyCode.R)) player.transform.position = _startPosition;
        }

        public void Respawn(GameObject checkPoint)
        {
            if (checkPoint == null)
                player.transform.position = _startPosition;
            else
            {
                Vector3 respawnPosition = new Vector3(checkPoint.transform.position.x, checkPoint.transform.position.y, player.transform.position.z);
                // player.transform.position = checkPoint.transform.position;
                player.transform.position = respawnPosition;
            }
        }
    }
}