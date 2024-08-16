using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private bool canDoubleJump;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isAirbone;
    private bool isWallDetected;

    private float xInput;

    private bool facingRight = true;
    private int facingDir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateAirboneStatus();

        HandleCollision();
        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleAnimation();
    }

    private void HandleWallSlide()
    {
        if(isWallDetected && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
    }

    private void UpdateAirboneStatus()
    {
        if (isGrounded && isAirbone)
            HandleLanding();

        if (!isGrounded && !isAirbone)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirbone = true;
    }

    private void HandleLanding()
    {
        isAirbone = false;
        canDoubleJump = true;
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
    }

    private void Jump() => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private void DoubleJump()
    {
        canDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, 
            Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, 
            Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void HandleMovement()
    {
        if (isWallDetected)
            return;

        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void HandleFlip()
    {
        if(xInput < 0 && facingRight || xInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}
