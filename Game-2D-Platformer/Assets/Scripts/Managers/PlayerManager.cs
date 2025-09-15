using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager {  get; private set; }
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

        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        if (this.player == null)
            this.player = player.GetComponent<Player>();

        OnPlayerRespawn?.Invoke();

        PlaceNewPlayerAtRespawnPoint(player.transform);
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    private void PlaceNewPlayerAtRespawnPoint(Transform newPlayer)
    {
        if (respawnPoint == null)
            respawnPoint = FindFirstObjectByType<StartPoint>().transform;

        newPlayer.position = respawnPoint.position;
    }
}
