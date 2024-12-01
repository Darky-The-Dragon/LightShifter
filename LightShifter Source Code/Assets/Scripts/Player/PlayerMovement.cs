using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private int availableJumps = 2;
        private Rigidbody2D _body;
        private int _jumpNumber;

        private PlayerState _state;

        private void Awake()
        {
            _state = PlayerState.Grounded;
        }

        private void Start()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            _body.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, _body.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && (_state == PlayerState.Grounded ||
                                                    (_state == PlayerState.InAir && _jumpNumber < availableJumps)))
            {
                _jumpNumber++;
                _body.velocity = new Vector2(_body.velocity.x, jumpForce);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _state = PlayerState.Grounded;
                _jumpNumber = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground")) _state = PlayerState.InAir;
        }

        private enum PlayerState
        {
            Grounded,
            InAir
        }
    }
}