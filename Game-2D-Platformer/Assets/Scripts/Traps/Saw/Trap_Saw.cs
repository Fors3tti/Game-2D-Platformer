using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private Transform[] waypoint;

    public int wayPointIndex = 1;
    private bool canMove = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = waypoint[0].position;
    }

    private void Update()
    {
        anim.SetBool("active", canMove);

        if (canMove == false)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position, waypoint[wayPointIndex].position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, waypoint[wayPointIndex].position) < .1f)
        {
            wayPointIndex++;

            if (wayPointIndex >= waypoint.Length)
            {
                wayPointIndex = 0;
                StartCoroutine(StopMovement(cooldown));
            }
        }
    }

    private IEnumerator StopMovement(float delay)
    {
        canMove = false;

        yield return new WaitForSeconds(delay);

        canMove = true;
    }
}
