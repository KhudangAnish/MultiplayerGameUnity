using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private Material nonInfectedMaterial;
   [SerializeField] private Material infectedMaterial;

    private bool isInfected = false;

    public bool IsInfected => isInfected;

    public void GetInfected()
    {
        isInfected = true;
        GetComponentInChildren<MeshRenderer>().material = infectedMaterial;
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
}
