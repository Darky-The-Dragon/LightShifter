using UnityEngine;
using UnityEngine.Audio;


public class OptionsScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);

    }   
    
    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);

    }    
    public void SFX_Volume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);

    }    public void UIVolume(float volume)
    {
        audioMixer.SetFloat("UI", volume);

    }
    
}
