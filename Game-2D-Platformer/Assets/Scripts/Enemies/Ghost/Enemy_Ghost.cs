using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Header("Ghost Details")]
    [SerializeField] private float activeDuration;
    private float activeTimer;

    private bool isChasing;

    protected override void Update()
    {
        base.Update();

        activeTimer -= Time.deltaTime;

        if(isChasing == false && idleTimer < 0)
        {
            StartChase();
        }
        else if (isChasing && activeTimer < 0)
        {
            EndChase();
        }
    }

    private void StartChase()
    {
        activeTimer = activeDuration;
        isChasing = true;
        anim.SetTrigger("appear");
    }

    private void EndChase()
    {
        idleTimer = idleDuration;
        isChasing = false;
        anim.SetTrigger("desappear");
    }
}
