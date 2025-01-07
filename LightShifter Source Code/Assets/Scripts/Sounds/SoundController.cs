using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.Play();
            }
                
        }
    }
}