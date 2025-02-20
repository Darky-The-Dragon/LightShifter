using System;
using System.Collections;
using UnityEngine;

namespace Player.Test
{
    public class PlayerMovementTest : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody2D_Component;
        [SerializeField] private Animator Animator_Component;
        private readonly float jumpPower = 5f;
        private readonly float moveSpeed = 5f;
        private Animator animator;
        private bool coyoteJump;
        private LayerMask ground;

        // Variables
        private float horizontalInput;
        private bool isFacingRight;
        private bool isGrounded;

        private Rigidbody2D rb;

        // Start is called before the first frame update
        private void Start()
        {
            //rb = GetComponent<Rigidbody2D>();
            //animator = GetComponent<Animator>();
            rb = Rigidbody2D_Component;
            animator = Animator_Component;
            ground = LayerMask.GetMask("Ground");
        }

        // Update is called once per frame
        private void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");

            FlipSprite();

            if (Input.GetButtonDown("Jump")) Jump();
        }

        // FixedUpdate is called before any execution of the engine physics
        private void FixedUpdate()
        {
            GroundCheck();

            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
        }


        private void FlipSprite()
        {
            if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
            {
                isFacingRight = !isFacingRight;
                var ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }

        private void GroundCheck()
        {
            var wasGrounded = isGrounded;
            isGrounded = false;

            if (rb.IsTouchingLayers(ground))
            {
                isGrounded = true;
            }
            else
            {
                if (wasGrounded) StartCoroutine(CoyoteJumpDelay());
            }

            animator.SetBool("isJumping", !isGrounded);
        }

        private IEnumerator CoyoteJumpDelay()
        {
            coyoteJump = true;
            yield return new WaitForSeconds(0.2f);
            coyoteJump = false;
        }

        private void Jump()
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //isGrounded = false;
                animator.SetBool("isJumping", !isGrounded);
            }
            else
            {
                if (coyoteJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    //isGrounded = false;
                    animator.SetBool("isJumping", !isGrounded);
                }
            }
        }
    }
}