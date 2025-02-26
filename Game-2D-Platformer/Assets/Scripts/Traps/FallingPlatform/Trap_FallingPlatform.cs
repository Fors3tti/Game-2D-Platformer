using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float travelDistance;
    public Vector3[] wayPoints;

    private void Start()
    {
        SetupWayPoints();
    }

    private void SetupWayPoints()
    {
        wayPoints = new Vector3[2];

        float yOffset = travelDistance / 2;

        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }
}
