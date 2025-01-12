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
        [SerializeField] private bool respawnDeveloper;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private float volume = 0.5f;
        private readonly List<ResetObject> _resetObjects = new();
        private Vector2 _startPosition;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _resetObjects.AddRange(FindObjectsOfType<ResetObject>());
        }

        private void Reset()
        {
            foreach (var resetObject in _resetObjects)
            {
                Debug.Log("Resetted game object: " + resetObject.gameObject.name);
                resetObject.Reset();
            }
        }

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


        public void Respawn(Vector2 checkPoint)
        {
            Reset();
            player.transform.position = checkPoint;
        }
    }
}