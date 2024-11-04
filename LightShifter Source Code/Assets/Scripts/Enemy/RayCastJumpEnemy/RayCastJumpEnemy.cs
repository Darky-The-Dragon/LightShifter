using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastJumpEnemy : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speed = 3f;

    [Header("Ground Detection Settings")]
    // The layer that the raycast, used to check ground presence, will detect
    [SerializeField] private LayerMask groundLayer;
    // The distance that the raycast will travel
    [SerializeField] private float groundDetectionDistance = 1f;
    private Rigidbody2D _rb;
    private Transform _raycastOriginTransform;
    private State _state;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _raycastOriginTransform = GetComponentInChildren<RayCastOrigin>().transform;

        if(_raycastOriginTransform == null) {
            Debug.LogError("Raycast origin not found");
        }
    }

    void FixedUpdate() {
        ConstantMovement();
        DetectAndJumpOverGap();
    }

    void ConstantMovement() {
        // transform.position += Vector3.right * speed * Time.deltaTime;
        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }

    void Jump() {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }
    /**
        <summary>
            Detects if there is a gap in front of the game object and jumps over it. The origin of the raycast is the child object, RayCastOrigin, of the game object.
        </summary>
    */
    void DetectAndJumpOverGap() {

        /*
            Generate a raycast to detect empty spaces on the ground in front of the game object.
            The groundLayer is the layer that the raycast will detect.
            The groundDetectionDistance is the distance that the raycast will travel.
            The raycastOrigin is the position of the RayCastOrigin child object, and is used as the starting point of the generated raycast.
        */
        Vector2 raycastOrigin = _raycastOriginTransform.position;
        RaycastHit2D raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundDetectionDistance, groundLayer);
        // if the raycast did not collide with anything and the game object is on the ground then jump
        if(raycastHit.collider == null && _state == State.Grounded) {
            Jump();
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            _state = State.Grounded;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            _state = State.InAir;
        }
    }
    private enum State {
        Grounded,
        InAir
    }


}
