using UnityEngine;
using UnityEngine.Audio;

public class UIAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;
    void Start()
    {
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.PlayOneShot(audioClip);
        audioSource.loop = true;
    }
    
}
