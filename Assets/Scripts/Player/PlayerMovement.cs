using UnityEngine;

namespace Player
{


    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _body;
        [SerializeField] float speed = 5f;
        [SerializeField] float jumpForce = 5f;

        private enum PlayerState
        {
            Grounded,
            InAir
        }

        private PlayerState _state;
        private int _jumpNumber;
        [SerializeField] private int availableJumps = 2;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            _state = PlayerState.Grounded;
        }

        // Update is called once per frame
        void Update()
        {
            _body.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, _body.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && (_state == PlayerState.Grounded ||
                                                    (_state == PlayerState.InAir && _jumpNumber < availableJumps)))
            {
                _jumpNumber++;
                _body.velocity = new Vector2(_body.velocity.x, jumpForce);
            }

        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _state = PlayerState.InAir;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _state = PlayerState.Grounded;
                _jumpNumber = 0;
            }
        }


    }

}
