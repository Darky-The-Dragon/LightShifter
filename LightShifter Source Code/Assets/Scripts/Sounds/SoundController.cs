using UnityEngine;

namespace Sounds
{
    public class SoundController : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip walkSound;
        public AudioClip jumpStart;
        public AudioClip jumpEnd;

        private void Start()
        {
            audioSource.clip = walkSound;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) audioSource.Play();
        }
    }
}