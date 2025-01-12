using System.Collections;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] private DoorBehaviour _doorBehaviour;

    [SerializeField] private bool _isDoorOpenSwitch;
    [SerializeField] private bool _isDoorClosedSwitch;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverTwistClip;

    [SerializeField] private AudioClip doorMovingClip;
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

        _switchUpPos = transform.position;
        _switchDownPos = new Vector3(transform.position.x, transform.position.y - _switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isPressingSwitch)
            MoveSwitchDown();
        else if (!_isPressingSwitch) MoveSwitchUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPressingSwitch = !_isPressingSwitch;

            _doorBehaviour._isDoorOpen = !_doorBehaviour._isDoorOpen;

            audioSource.PlayOneShot(leverTwistClip);

            DoorSound();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) StartCoroutine(SwitchUpDelay(_switchDelay));
    }

    private void MoveSwitchDown()
    {
        if (transform.position != _switchDownPos)
            transform.position = Vector3.MoveTowards(transform.position, _switchDownPos, _switchSpeed * Time.deltaTime);
    }

    private void MoveSwitchUp()
    {
        if (transform.position != _switchUpPos)
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