using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino Details")]
    [SerializeField] private float detectionRange;
    private bool playerDetected;

    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.velocity.x);

        HandleCollision();
        HandleCharge();
    }

    private void HandleCharge()
    {
        if (canMove == false)
            return;

        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position,
            Vector2.right * facingDir, detectionRange, whatIsPlayer);

        if (playerDetected)
            canMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
