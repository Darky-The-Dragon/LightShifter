using TarodevController.Demo;
using UnityEngine;

public class SwitchPlatformBehavior : ResetObject
{

    public PatrolPlatform patrolPlatform;
    [Tooltip("If true, the platform will start moving when the switch is pressed, otherwise it will stop moving")]
    public bool switchOn = true;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverTwistClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(leverTwistClip);
            gameObject.SetActive(false);
            patrolPlatform.EnableMovement(switchOn);
        }
    }

    public override void Reset()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Resetting switch of " + patrolPlatform.name);
        patrolPlatform.Reset();
    }

    void OnValidate()
    {
        if (patrolPlatform == null)
        {
            Debug.LogError("Patrol platform is not set for " + this.name);
        }
    }
}
