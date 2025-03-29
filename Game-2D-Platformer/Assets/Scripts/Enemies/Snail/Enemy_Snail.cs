using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail : Enemy
{
    [Header("Snail Details")]
    [SerializeField] private Enemy_Snail_Body bodyPrefab;
    [SerializeField] private float maxSpeed = 10f;
    private bool hasBody = true;

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
        else if(canMove == false && hasBody == false)
        {
            anim.SetTrigger("hit");
            canMove = true;
            moveSpeed = maxSpeed;
        }
        else
        {
            base.Die();
        }
    }

    private void HandleTurnAround()
    {
        if (!isGroundInFrontDetected && hasBody || isWallDetected)
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

    private void CreateBody()
    {
        Enemy_Snail_Body newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity);

        if (Random.Range(0, 100) < 50)
            deathRotationDirection = deathRotationDirection * -1;

        newBody.SetupBody(deathImpactSpeed, deathRotationSpeed * deathRotationDirection, facingDir);

        Destroy(newBody.gameObject, 10);
    }

    protected override void Flip()
    {
        base.Flip();

        if (hasBody == false)
            anim.SetTrigger("wallHit");
    }
}
