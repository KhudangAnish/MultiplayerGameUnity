using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private Material nonInfectedMaterial;
   [SerializeField] private Material infectedMaterial;

    private bool isInfected = false;

    public bool IsInfected => isInfected;

    private void Awake()
    {
       // GetComponentInChildren<MeshRenderer>().material = nonInfectedMaterial;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"client id{GetComponent<NetworkObject>().OwnerClientId}");
        Debug.Log($"network id{GetComponent<NetworkObject>().NetworkObjectId}");

    }

    //private void Update()
    //{
    //    if(isInfected)
    //    {
    //        //Logic for effected
    //    }
    //    else
    //    {
    //        //If not effected do what?
    //    }
    //}

    public void GetInfected()
    {
        isInfected = true;
        Debug.Log($"Player {GetComponent<NetworkObject>().OwnerClientId} has been infected");
        GetComponentInChildren<MeshRenderer>().material = infectedMaterial;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(isInfected)
        {
           //If collision is player
           if(collision.collider.GetComponent<PlayerController>())
            {
                var playerToInfect = collision.collider.GetComponent<PlayerController>();
                if (playerToInfect.IsInfected == true) return;

                //If its not infected

                Debug.Log("Find person to infect with the woke virus");
                var ownerClientId = (int)playerToInfect.GetComponent<NetworkObject>().OwnerClientId;
                Debug.Log("players id is " + ownerClientId);
                if (GameManager.Instance == null) Debug.Log("GameManager is null?");
                GameManager.Instance.InfectPersonServerRpc(ownerClientId);

            }
        }
    }
}
