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
  //  private List<NetworkObject> listOfPlayers;



    public override void OnNetworkSpawn()
    {
        // Add to playercount

        if(IsHost)
        {
            playerCount.Value = 1;
        }
        else
        {
            ChangePlayerCountServerRpc(1);
        }
     
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
    private void InfectPeople()
    {
        //Randomly choose a person to infect
        int infectedPersonID = Random.Range(0, playerCount.Value);

        //Ch
    }
}
