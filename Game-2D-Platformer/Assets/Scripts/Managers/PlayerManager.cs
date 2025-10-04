using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager { get; private set; }
    public static event Action OnPlayerRespawn;
    public static PlayerManager instance;

    public int lifePoints;
    public int maxPlayerCount = 1;
    [Header("Player")]
    [SerializeField] private List<Player> playerList = new List<Player>();
    [SerializeField] private Transform respawnPoint;
    public Player player;
    [SerializeField] private string[] playerDevice;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.SetMaximumPlayerCount(maxPlayerCount);
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
        playerInputManager.onPlayerLeft += RemovePlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
        playerInputManager.onPlayerLeft -= RemovePlayer;
    }

    private void AddPlayer(PlayerInput newPlayer)
    {
        if (this.player == null)
            this.player = newPlayer.GetComponent<Player>();

        Player playerScript = newPlayer.GetComponent<Player>();

        playerList.Add(playerScript);

        OnPlayerRespawn?.Invoke();
        PlaceNewPlayerAtRespawnPoint(newPlayer.transform);

        int newPlayerNumber = GetPlayerNumber(newPlayer);
        int newPlayerSkinId = SkinManager.instance.GetSkinId(newPlayerNumber);

        playerScript.UpdateSkin(newPlayerSkinId);
    }

    private void RemovePlayer(PlayerInput player)
    {
        Player playerScript = player.GetComponent<Player>();
        playerList.Remove(playerScript);

        lifePoints--;

        if (lifePoints <= 0)
        {
            playerInputManager.DisableJoining();
            GameManager.instance.RestartLevel();
        }

        UI_InGame.instance.UpdateLifePointsUI(lifePoints, maxPlayerCount);
    }

    private int GetPlayerNumber(PlayerInput newPlayer)
    {
        int newPlayerNumber = 0;

        foreach (var device in newPlayer.devices)
        {
            for (int i = 0; i < playerDevice.Length; i++)
            {
                if (playerDevice[i] == "Empty")
                {
                    newPlayerNumber = i;
                    playerDevice[i] = device.name;
                    break;
                }
                else if (playerDevice[i] == device.name)
                {
                    newPlayerNumber = i;
                    break;
                }
            }
        }

        return newPlayerNumber;
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    private void PlaceNewPlayerAtRespawnPoint(Transform newPlayer)
    {
        if (respawnPoint == null)
            respawnPoint = FindFirstObjectByType<StartPoint>().transform;

        newPlayer.position = respawnPoint.position;
    }
}
