using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



public enum State
{
    Setup,
    Playing,
    Ended
}
public class GameManager : NetworkBehaviour
{
    public State CurrentGameState = State.Setup;

    private static GameManager instance;
  
    public static GameManager Instance { get => instance; set => instance = value; }


    [SerializeField] private SetupMenu setupMenu;
    [SerializeField] private EndedState endedState;

    public NetworkVariable<bool> allIsInfected = new NetworkVariable<bool>(false);
    public override void OnNetworkSpawn()
    {
        InitSingleton();
    }
    private void InitSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    

    private void Update()
    {
        switch (CurrentGameState)
        {
            case State.Setup:
                setupMenu.UpdateSetup();
                break;
            case State.Playing:
                Playing();
                break;
            case State.Ended:
                //Give scores or something
                endedState.UpdateState();
                break;
        }
    }
    private void Playing()
    {
        if (IsServer is false) return;
        CheckTimer();
        CheckInfected();
    }
    private float maxTime = 120; // 120 seconds
    private float currentTime = 0;
    private void CheckTimer()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= maxTime)
        {
            currentTime = 0;
            ChangeStateRpc(State.Ended);
        }
    }
    private void CheckInfected()
    {
        var getPlayers = NetworkManager.Singleton.ConnectedClientsList;

        bool allInfected = true;
        foreach (var player in getPlayers)
        {
            if(player.PlayerObject.GetComponent<PlayerController>().IsInfected is false)
            {
                allInfected = false;
                break;
            }
        }
        if(allInfected) 
        {
            allIsInfected.Value = true;
            ChangeStateRpc(State.Ended);
        }
    }
    public void StartGame()
    {
        ChangeStateRpc(State.Playing);
        RandomlyChooseAPersonToInfect();
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void ChangeStateRpc(State state)
    {
        if (state == CurrentGameState) return;
        CurrentGameState = state;
    }
    private void RandomlyChooseAPersonToInfect()
    {
        //Randomly choose a person to infect
        int infectedPersonID = Random.Range(0, NetworkManager.Singleton.ConnectedClients.Count);
        InfectRpc(infectedPersonID);
    }

    [Rpc(SendTo.Server)]
    public void InfectPersonServerRpc(int infectedPersonID)
    {
        InfectRpc(infectedPersonID);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void InfectRpc(int infectedPersonID)
    {
        //Person that gets infected
       // NetworkObject infectedPerson = NetworkManager.Singleton.SpawnManager.SpawnedObjects[(ulong)infectedPersonID];
        NetworkObject infectedPerson = NetworkManager.Singleton.ConnectedClients[(ulong)infectedPersonID].PlayerObject;
        infectedPerson.GetComponent<PlayerController>().GetInfected();
    }
}
