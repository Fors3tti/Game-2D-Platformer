using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant Details")]
    [SerializeField] private float attackCooldown;
    private float lastTimeAttacked;

    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayerDetected && canAttack)
            Attack();
    }

    private void Attack()
    {
        lastTimeAttacked = Time.time;
        anim.SetTrigger("attack");
    }

    protected override void HandleAnimator()
    {
        //Keep it empty, unless you need to update parameters
    }
}
