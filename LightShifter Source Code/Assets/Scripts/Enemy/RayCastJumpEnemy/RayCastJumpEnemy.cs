using UnityEngine;

public class RayCastJumpEnemy : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float jumpForce = 5f;

    [SerializeField] private float speed = 3f;

    [Header("Ground Detection Settings")]
    // The layer that the raycast, used to check ground presence, will detect
    [SerializeField]
    private LayerMask groundLayer;

    // The distance that the raycast will travel
    [SerializeField] private float groundDetectionDistance = 1f;
    private Transform _raycastOriginTransform;
    private Rigidbody2D _rb;
    private State _state;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _raycastOriginTransform = GetComponentInChildren<RayCastOrigin>().transform;

        if (_raycastOriginTransform == null) Debug.LogError("Raycast origin not found");
    }

    private void FixedUpdate()
    {
        ConstantMovement();
        DetectAndJumpOverGap();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) _state = State.Grounded;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) _state = State.InAir;
    }

    private void ConstantMovement()
    {
        // transform.position += Vector3.right * speed * Time.deltaTime;
        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }

    /**
     * <summary>
     *     Detects if there is a gap in front of the game object and jumps over it. The origin of the raycast is the child
     *     object, RayCastOrigin, of the game object.
     * </summary>
     */
    private void DetectAndJumpOverGap()
    {
        /*
            Generate a raycast to detect empty spaces on the ground in front of the game object.
            The groundLayer is the layer that the raycast will detect.
            The groundDetectionDistance is the distance that the raycast will travel.
            The raycastOrigin is the position of the RayCastOrigin child object, and is used as the starting point of the generated raycast.
        */
        Vector2 raycastOrigin = _raycastOriginTransform.position;
        var raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundDetectionDistance, groundLayer);
        // if the raycast did not collide with anything and the game object is on the ground then jump
        if (raycastHit.collider == null && _state == State.Grounded) Jump();
    }

    private enum State
    {
        Grounded,
        InAir
    }
}