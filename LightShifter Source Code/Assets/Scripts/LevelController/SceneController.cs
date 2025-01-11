using System.Collections;
using System.Collections.Generic;
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
        private List<ResetObject> _resetObjects = new List<ResetObject>();
        [SerializeField] private bool respawnDeveloper = false;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _resetObjects.AddRange(FindObjectsOfType<ResetObject>());
        }
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private float volume = 0.5f;

        private void Start()
        {
            _startPosition = player.transform.position;
            
            //Sounds
            if (audioSource == null)
            {
                Debug.LogError("AudioSource not assigned!");
                return;
            }

            // Assign and configure the AudioSource
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.volume = volume;

            // Play the music
            audioSource.Play();
        }

        private void Update()
        {
            RespawnDevelopers();
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

        private void RespawnDevelopers()
        {

            if (Input.GetKeyDown(KeyCode.R) && respawnDeveloper) player.transform.position = _startPosition;
        }

        private void Reset()
        {
            foreach (ResetObject resetObject in _resetObjects)
            {
                Debug.Log("Resetted game object: " + resetObject.gameObject.name);
                resetObject.Reset();
            }
        }


        public void Respawn(Vector2 checkPoint)
        {
            Reset();
            player.transform.position = checkPoint;
        }
    }
}