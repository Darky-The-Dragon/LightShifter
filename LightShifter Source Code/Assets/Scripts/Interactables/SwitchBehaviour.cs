using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] private List<DoorBehaviour> _doorBehaviours;

    [SerializeField] private bool _isDoorOpenSwitch;
    [SerializeField] private bool _isDoorClosedSwitch;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverTwistClip;

    [SerializeField] private AudioClip doorMovingClip;
    [SerializeField] private KeyCode switchKey = KeyCode.E;
    [SerializeField] private Transform playerCenter;
    [SerializeField] private float activationRange = 0.5f;
    private bool _isSwitchActive;

    private readonly float _switchDelay = 0.2f;
    private readonly float _switchSpeed = 1f;

    private bool _isPressingSwitch;
    private Vector3 _switchDownPos;

    private float _switchSizeY;
    private Vector3 _switchUpPos;


    // Start is called before the first frame update
    private void Awake()
    {
        _switchSizeY = transform.localScale.y / 2;
        _isSwitchActive = false;
        _switchUpPos = transform.position;
        _switchDownPos = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     if (_isPressingSwitch)
    //         MoveSwitchDown();
    //     else if (!_isPressingSwitch) MoveSwitchUp();
    // }

    private void Update()
    {
        if (_isSwitchActive)
            MoveSwitchDown();
        else
            MoveSwitchUp();

        if (Input.GetKeyDown(switchKey) && IsPlayerInRange())
        {
            Debug.Log(gameObject.name + " isSwitchActive: " + _isSwitchActive);
            ToggleSwitch();

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
        float distance = Vector2.Distance(transform.position, playerCenter.position);
        // Debug.Log(gameObject.name + " distance to the player: " + distance);
        return distance <= activationRange;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         _isPressingSwitch = !_isPressingSwitch;

    //         _doorBehaviour._isDoorOpen = !_doorBehaviour._isDoorOpen;

    //         audioSource.PlayOneShot(leverTwistClip);

    //         DoorSound();
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player")) StartCoroutine(SwitchUpDelay(_switchDelay));
    // }

    private void MoveSwitchDown()
    {
        Debug.Log(gameObject.name + " moving switch down");
        Debug.Log(gameObject.name + " switchDownPos: " + _switchDownPos);
        // if (transform.position != _switchDownPos)
        transform.position = Vector3.MoveTowards(transform.position, _switchDownPos, _switchSpeed * Time.deltaTime);
    }

    private void MoveSwitchUp()
    {
        Debug.Log(gameObject.name + " moving switch up");
        Debug.Log(gameObject.name + " switchUpPos: " + _switchUpPos);
        // if (transform.position != _switchUpPos)
        transform.position = Vector3.MoveTowards(transform.position, _switchUpPos, _switchSpeed * Time.deltaTime);
    }

    private IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isPressingSwitch = false;
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