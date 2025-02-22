using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] waypoint;

    public int wayPointIndex = 1;

    private void Start()
    {
        transform.position = waypoint[0].position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, waypoint[wayPointIndex].position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, waypoint[wayPointIndex].position) < .1f)
        {
            wayPointIndex++;

            if (wayPointIndex >= waypoint.Length)
                wayPointIndex = 0;
        }
    }
}
