using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;

    private float xInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        HandleAnimation();

        HandleMovement();
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }
}
