using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino Details")]
    [SerializeField] private float detectionRange;
    private bool playerDetected;

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position,
            Vector2.right * facingDir, detectionRange, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
