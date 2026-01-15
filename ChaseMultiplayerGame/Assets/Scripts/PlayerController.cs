using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private GameObject nonInfected;
   [SerializeField] private GameObject infected;

    private bool isInfected = false;

    public bool IsInfected => isInfected;
    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            CameraController.Instance.InitializeCamera(transform);
        }
    }
    private void Start()
    {
        Reset();
    }

    public void GetInfected()
    {
        isInfected = true;
        nonInfected.SetActive(false);
        infected.SetActive(true);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isInfected is false) return;
        if (collision.collider.TryGetComponent(out PlayerController playerToInfect) is false) return;
        if (playerToInfect.IsInfected == true) return;

        //If its not already infected
        var ownerClientId = (int)playerToInfect.GetComponent<NetworkObject>().OwnerClientId;
        GameManager.Instance.InfectPersonServerRpc(ownerClientId);


    }

    public void Reset()
    {
        transform.position = GameManager.Instance.spwawnPosition[Random.Range(0, GameManager.Instance.spwawnPosition.Length)].position;

        nonInfected.SetActive(true);
        infected.SetActive(false);

        isInfected = false;
    }

}
