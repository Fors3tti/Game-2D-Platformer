using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail : Enemy
{
    public bool hasBody = true;

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        HandleMovement();

        if (isGrounded)
        {
            HandleTurnAround();
        }
    }

    public override void Die()
    {
        if (hasBody)
        {
            canMove = false;
            hasBody = false;
            anim.SetTrigger("hit");

            rb.velocity = Vector2.zero;
            idleDuration = 0;
        }
        else
        {
            base.Die();
        }
    }

    private void HandleTurnAround()
    {
        if (!isGroundInFrontDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;

        if (canMove == false)
            return;

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
}
