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
    private NetworkVariable<int> playerCount = new NetworkVariable<int>();
    // private NetworkVariable<List<NetworkObject>> listOfPlayers = new NetworkVariable<List<NetworkObject>>();

    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }


  

    private void Awake()
    {
        Debug.Log("DO I RUN?");
    }

    public override void OnNetworkSpawn()
    {
        // Add to playercount
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (IsHost)
        {
            playerCount.Value = 1;
            //  listOfPlayers.Value.Add()
        }
        else
        {
            ChangePlayerCountServerRpc(1);
        }

       // Debug.Log(GetComponent<NetworkObject>().OwnerClientId)
       // Debug.Log(GameObject.Find("Player(Clone)").GetComponent<NetworkObject>().OwnerClientId);


    }
    public override void OnNetworkDespawn()
    {
        if (!IsHost)
        {
            ChangePlayerCountServerRpc(-1);
        }
    }




    private void Update()
    {
        switch (CurrentGameState)
        {
            case State.Setup:
                if (!IsHost) return;

                if (Input.GetKeyDown(KeyCode.Space) && playerCount.Value > 1)
                {
                    StartGame();
                }
                break;
            case State.Playing:
                Debug.Log("Playing");
                break;
            case State.Ended:

                break;
        }
    }

    private void StartGame()
    {
        StartGameClientRpc();
        RandomlyChooseAPersonToInfect();
    }

    [Rpc(SendTo.Server)]
    private void ChangePlayerCountServerRpc(int player)
    {
        playerCount.Value += player;
    }


    [Rpc(SendTo.ClientsAndHost)]
    private void StartGameClientRpc()
    {
        CurrentGameState = State.Playing;
    }
    private void RandomlyChooseAPersonToInfect()
    {
        //Randomly choose a person to infect
        int infectedPersonID = Random.Range(0, playerCount.Value + 1);
        InfectRpc(infectedPersonID);
    }

    [Rpc(SendTo.Server)]
    public void InfectPersonServerRpc(int infectedPersonID)
    {
        Debug.Log("Arrived in server");
        InfectRpc(infectedPersonID);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void InfectRpc(int infectedPersonID)
    {
        //Person that gets infected
        Debug.Log("INFECTING RPC");
        NetworkObject infectedPerson = NetworkManager.Singleton.SpawnManager.SpawnedObjects[(ulong)infectedPersonID];
        infectedPerson.GetComponent<PlayerController>().GetInfected();
    }
}
