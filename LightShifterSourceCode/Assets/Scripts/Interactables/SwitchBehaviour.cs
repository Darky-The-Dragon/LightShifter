using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField, Tooltip("Set to true if the interactable is to be activated through the selected key, false if it is to be activated through collision")] private bool activateWithKey;
    [SerializeField, Tooltip("List of doors to be handled")] private List<DoorBehaviour> _doorBehaviours;
    [SerializeField] private KeyCode switchKey = KeyCode.E;
    [SerializeField, Tooltip("The bounding box of the player used to detect custom collisions")] private Transform playerBoundingBox;
    [SerializeField] private float activationRange = 0.5f;
    /* commented as weren't used in the script */
    // [SerializeField] private bool _isDoorOpenSwitch;
    // [SerializeField] private bool _isDoorClosedSwitch;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverTwistClip;

    [SerializeField] private AudioClip doorMovingClip;


    [Header("Animation")]
    [SerializeField, Tooltip("The delay before the switch moves back up after being activated")] private float switchDelay = 0.2f;
    [SerializeField, Tooltip("Movement speed during the switch animation")] private float switchSpeed = 1f;
    [SerializeField, Tooltip("Y offset by which the sprite moves down when activated. Setting negative values means to move the switch up, rather than down")] private float switchOffset_Y = 0.3f;

    private bool _isSwitchActive;
    // private bool _isPressingSwitch;
    private Vector3 _switchDownPos;

    // private float _switchSizeY;
    private Vector3 _switchUpPos;


    // Start is called before the first frame update
    private void Awake()
    {
        // _switchSizeY = transform.localScale.y / 2;
        _isSwitchActive = false;
        _switchUpPos = transform.position;
        _switchDownPos = new Vector3(transform.position.x, transform.position.y - switchOffset_Y, transform.position.z);
    }

    private void Update()
    {
        if (_isSwitchActive)
            MoveSwitchDown();
        else
            MoveSwitchUp();

        /* toggle the switch using the proper key only if the functionality is selected through the inspector */
        if (activateWithKey && Input.GetKeyDown(switchKey) && IsPlayerInRange())
        {
            ToggleSwitch();
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }

    private void ToggleSwitch()
    {
        // _isPressingSwitch = !_isPressingSwitch;
        _isSwitchActive = !_isSwitchActive;
        foreach (DoorBehaviour doorBehaviour in _doorBehaviours)
        {
            doorBehaviour._isDoorOpen = !doorBehaviour._isDoorOpen;
        }
        audioSource.PlayOneShot(leverTwistClip);
        DoorSound();
    }

    private bool IsPlayerInRange()
    {
        float distance = Vector2.Distance(transform.position, playerBoundingBox.position);
        return distance <= activationRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activateWithKey && (collision.CompareTag("PlayerBoundingBox") || collision.CompareTag("MovingBlock")))
        {
            Debug.Log(gameObject.name + " entered switch range");
            ToggleSwitch();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!activateWithKey && (collision.CompareTag("PlayerBoundingBox") || collision.CompareTag("MovingBlock")))
        {
            Debug.Log(gameObject.name + " exited switch range");
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }

    private void MoveSwitchDown()
    {
        // Debug.Log(gameObject.name + " moving switch down");
        // Debug.Log(gameObject.name + " switchDownPos: " + _switchDownPos);
        transform.position = Vector3.MoveTowards(transform.position, _switchDownPos, switchSpeed * Time.deltaTime);
    }

    private void MoveSwitchUp()
    {
        // Debug.Log(gameObject.name + " moving switch up");
        // Debug.Log(gameObject.name + " switchUpPos: " + _switchUpPos);
        transform.position = Vector3.MoveTowards(transform.position, _switchUpPos, switchSpeed * Time.deltaTime);
    }

    private IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // _isPressingSwitch = false;
        _isSwitchActive = false;
    }

    private void DoorSound()
    {
        audioSource.clip = doorMovingClip;
        audioSource.loop = true;
        audioSource.Play();
        StartCoroutine(StopDoorSound(1.5f));
    }

    private IEnumerator StopDoorSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.Stop();
    }
}