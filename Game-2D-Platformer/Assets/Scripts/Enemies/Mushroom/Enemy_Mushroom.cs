using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.velocity.x);

        HandleCollision();
        HandleMovement();

        if (!isGroundDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

}
