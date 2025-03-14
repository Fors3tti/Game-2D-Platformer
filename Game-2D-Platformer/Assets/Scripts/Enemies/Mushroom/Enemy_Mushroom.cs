using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);

        HandleMovement();
        HandleCollision();

        if (!isGroundDetected)
            Flip();
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
}
