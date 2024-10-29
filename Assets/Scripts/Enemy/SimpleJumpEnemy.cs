using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : MonoBehaviour
{

    private Rigidbody2D _rb;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speed = 3f;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ConstantMovement();
    }

    void ConstantMovement() {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void Jump() {
        _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other) {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("JumpTrigger")) {
            Jump();
        }
    }
}
