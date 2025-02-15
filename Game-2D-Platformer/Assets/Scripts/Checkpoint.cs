using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private bool active;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
            return;

        Player player = collision.GetComponent<Player>();

        if (player != null)
            ActivateCheckpoint();
    }

    private void ActivateCheckpoint()
    {
        active = true;
        anim.SetBool("activate", active);
        GameManager.instance.UpdateRespawnPosition(transform);
    }
}
