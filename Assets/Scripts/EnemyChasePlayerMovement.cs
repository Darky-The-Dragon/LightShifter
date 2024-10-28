using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChasePlayerMovement : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 4f;
    private Rigidbody2D _rb;
    private bool _touchingPlayer;
    void Awake() {
        playerTransform = FindObjectOfType<Player>().transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        FollowPlayer();
    }

    void FollowPlayer() {
        if(!_touchingPlayer) {
            Vector2 chasePosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, chasePosition, speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            _touchingPlayer = true;            
        }
    }

    
    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            _touchingPlayer = false;            
        }
    }

}
