using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class EnemyChasePlayerMovement : MonoBehaviour
{
    [Tooltip("Transform of the player to chase")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 4f;
    private Rigidbody2D _rb;
    private bool _touchingPlayer;
    void Awake() {
        // Find the player object in the scene and get its transform
        playerTransform = FindObjectOfType<PlayerCollision>().transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        FollowPlayer();
    }
    /**
        <summary>
            Moves the object towards the player's x position. If the object is touching the chased player, it will stop moving.
        </summary>
    */
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
