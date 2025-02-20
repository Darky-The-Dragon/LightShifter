using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speed = 3f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ConstantMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("JumpTrigger"))
        {
            var jumpPower = other.gameObject.GetComponent<JumpTrigger>().jumpPower;
            print("Jumping with power: " + jumpPower);
            Jump(jumpPower);
        }
    }

    private void ConstantMovement()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Jump(float jumpForce)
    {
        _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}