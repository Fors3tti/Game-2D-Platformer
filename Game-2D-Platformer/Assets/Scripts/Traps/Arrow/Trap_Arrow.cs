using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Arrow : Trap_Trampoline
{
    [Header("Additional Info")]
    [SerializeField] private float cooldown;
    [SerializeField] private bool rotationRight;
    [SerializeField] private float rotationSpeed;

    private int direction = -1;

    private void Update()
    {
        direction = rotationRight ? -1 : 1;

        transform.Rotate(0, 0, (rotationSpeed * direction) * Time.deltaTime);
    }

    private void DestroyMe()
    {
        GameObject arrowPrefab = GameManager.instance.arrowPrefab;
        GameManager.instance.CreateObject(arrowPrefab, transform, cooldown);

        Destroy(gameObject);
    }
}
