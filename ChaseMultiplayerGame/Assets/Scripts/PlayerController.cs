using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private GameObject nonInfected;
   [SerializeField] private GameObject infected;

    private bool isInfected = false;

    public bool IsInfected => isInfected;
    public override void OnNetworkSpawn()
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
        transform.position = Random.insideUnitSphere * 20;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        nonInfected.SetActive(true);
        infected.SetActive(false);

        isInfected = false;
    }

}
