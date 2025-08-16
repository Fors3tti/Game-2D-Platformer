using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerRespawn;
    public static PlayerManager instance;

    [Header("Player")]
    [SerializeField] private Transform respawnPoint;
    public Player player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (respawnPoint == null)
            respawnPoint = FindFirstObjectByType<StartPoint>().transform;

        if (player == null)
            player = FindFirstObjectByType<Player>();

        //OnPlayerRespawn?.Invoke();
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
}
