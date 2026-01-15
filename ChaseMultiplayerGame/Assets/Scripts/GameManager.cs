using System.Collections;
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
    //These two need to reset on all of the clients and host
    //making currentstate as network variable
    public NetworkVariable<State> CurrentGameState =
    new NetworkVariable<State>(State.Setup);
    public NetworkVariable<bool> allIsInfected =
    new NetworkVariable<bool>(false);

    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }


    [SerializeField] private SetupMenu setupMenu;
    [SerializeField] private EndedState endedState;

    [SerializeField]public  Transform[] spwawnPosition;
    public override void OnNetworkSpawn()
    {
        InitSingleton();
    }
    private void InitSingleton()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Update()
    {
        switch (CurrentGameState.Value)
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
        if (NetworkManager.Singleton == null) return;
        var getPlayers = NetworkManager.Singleton.ConnectedClientsList;

        bool allInfected = true;
        foreach (var player in getPlayers)
        {
            if (player.PlayerObject.GetComponent<PlayerController>().IsInfected is false)
            {
                allInfected = false;
                break;
            }
        }
        if (allInfected)
        {
            allIsInfected.Value = true;
            ChangeStateRpc(State.Ended);
            StartCoroutine(DelayRestart());
        }
    }

    IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(1f);
        OnGameRestart_ClientRpc();
        allIsInfected.Value = false;
        yield return new WaitForSeconds(1f);
        currentTime = 0;
        StartGame();
    }

    [ClientRpc]
    public void OnGameRestart_ClientRpc()
    {
        var getPlayers = NetworkManager.Singleton.ConnectedClientsList;
        foreach (var player in getPlayers)
        {
            player.PlayerObject.GetComponent<PlayerController>().Reset();
        }
    }

    public void StartGame()
    {
        ChangeStateRpc(State.Playing);
        RandomlyChooseAPersonToInfect();
    }

    public void ChangeStateRpc(State state)
    {
        if (state == CurrentGameState.Value) return;
        CurrentGameState.Value = state;
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





    //Used to destroy/Despawn objects on the network for everyone
    public void DespawnAGameObject(ulong despawnGameObject)
    {
        DespawnAGameObjectRpc(despawnGameObject);
    }

    [Rpc(SendTo.Server)]
    private void DespawnAGameObjectRpc(ulong despawnGameObject)
    {
        // NetworkManager.Singleton.
        NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(despawnGameObject, out NetworkObject n);
        n.Despawn(true);
        //if (despawnGameObject.TryGet(out NetworkObject n)) return;
        // n.Despawn(true);
    }
}
