using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool _isDoorOpen;
    private Vector3 _doorClosedPos;
    private Vector3 _doorOpenPos;

    private readonly float _doorSpeed = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        _doorClosedPos = transform.position;
        _doorOpenPos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isDoorOpen)
            OpenDoor();
        else if (!_isDoorOpen) CloseDoor();
    }

    private void OpenDoor()
    {
        if (transform.position != _doorOpenPos)
            transform.position = Vector3.MoveTowards(transform.position, _doorOpenPos, _doorSpeed * Time.deltaTime);
    }

    private void CloseDoor()
    {
        if (transform.position != _doorClosedPos)
            transform.position = Vector3.MoveTowards(transform.position, _doorClosedPos, _doorSpeed * Time.deltaTime);
    }
}