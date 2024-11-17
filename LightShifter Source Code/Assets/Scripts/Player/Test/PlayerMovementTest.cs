using System;
using System.Collections;
using UnityEngine;

namespace Player.Test
{
    public class PlayerMovementTest : MonoBehaviour
    {
        [SerializeField] Rigidbody2D Rigidbody2D_Component;
        [SerializeField] Animator Animator_Component; 
        
        // Variables
        float horizontalInput;
        float moveSpeed = 5f;
        bool isFacingRight = false;
        float jumpPower = 5f;
        bool isGrounded = false;
        bool coyoteJump;
        private LayerMask ground;

        Rigidbody2D rb;
        Animator animator;
        
        // Start is called before the first frame update
        void Start()
        {
            //rb = GetComponent<Rigidbody2D>();
            //animator = GetComponent<Animator>();
            rb = Rigidbody2D_Component;
            animator = Animator_Component;
            ground = LayerMask.GetMask("Ground");
        }
        
        // FixedUpdate is called before any execution of the engine physics
        void FixedUpdate()
        {
            GroundCheck();
            
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // Update is called once per frame
        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");

            FlipSprite();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        

        void FlipSprite()
        {
            if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }

        void GroundCheck()
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;
            
            if (rb.IsTouchingLayers(ground))
            {
                isGrounded = true;
            }
            else
            {
                if (wasGrounded)
                {
                    StartCoroutine(CoyoteJumpDelay());
                }
            }
            
            animator.SetBool("isJumping", !isGrounded);
        }

        IEnumerator CoyoteJumpDelay()
        {
            coyoteJump = true;
            yield return new WaitForSeconds(0.2f);
            coyoteJump = false;
        }

        void Jump()
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
        }
    }
}
