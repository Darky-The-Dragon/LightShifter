using UnityEngine;

public class EnemyChasePlayerMovement : MonoBehaviour
{
    [Tooltip("Transform of the player to chase")] [SerializeField]
    private Transform playerTransform;

    [SerializeField] private float speed = 4f;
    private Rigidbody2D _rb;
    private bool _touchingPlayer;

    private void Awake()
    {
        // Find the player object in the scene and get its transform
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) _touchingPlayer = true;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) _touchingPlayer = false;
    }

    /**
        <summary>
            Moves the object towards the player's x position. If the object is touching the chased player, it will stop moving.
        </summary>
    */
    private void FollowPlayer()
    {
        if (!_touchingPlayer)
        {
            var chasePosition = new Vector2(playerTransform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, chasePosition, speed * Time.fixedDeltaTime);
        }
    }
}