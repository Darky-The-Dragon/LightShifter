using TarodevController.Demo;
using UnityEngine;
using System.Collections.Generic;

public class SwitchPlatformBehavior : ResetObject
{
    [SerializeField] private List<PatrolPlatform> patrolPlatforms;

    [Tooltip("If true, the platform will start moving when the switch is pressed, otherwise it will stop moving")]
    public bool switchOn = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverTwistClip;

    public override void Reset()
    {
        gameObject.SetActive(true);
        // Debug.Log("Resetting switch of " + patrolPlatform.name);
        foreach(PatrolPlatform x in patrolPlatforms) 
            x.Reset();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(leverTwistClip);
            gameObject.SetActive(false);
            foreach(PatrolPlatform x in patrolPlatforms) 
                x.EnableMovement(switchOn);
        }
    }

    private void OnValidate()
    {
        if (patrolPlatforms == null || patrolPlatforms.Contains(null)) Debug.LogError("Patrol platform is not set for " + name);
    }
}