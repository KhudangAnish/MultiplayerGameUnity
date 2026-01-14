using Unity.Netcode;
using UnityEngine;

public class SetupMenu : NetworkBehaviour
{
    private const int minimumPlayersToStart = 2;
    [SerializeField] private GameObject startGameUI;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            NetworkManager.Singleton.OnConnectionEvent += GameManager_OnConnectionEvent;
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
            NetworkManager.Singleton.OnConnectionEvent -= GameManager_OnConnectionEvent;
    }

    private void GameManager_OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
    {
        CheckIfCanStart();
    }

    public void UpdateSetup()
    {
        //Maybe we want to update a list of scores or what players have joined
    }

    private void CheckIfCanStart()
    {
        int connectedClients = NetworkManager.Singleton.ConnectedClients.Count;
        if (connectedClients >= minimumPlayersToStart)
            startGameUI.SetActive(true);
        else
            startGameUI.SetActive(false);
    }
    public void OnStartGame()
    {
        startGameUI.SetActive(false);
        GameManager.Instance.StartGame();
    }
}
