using Unity.Netcode;
using UnityEngine;

public class PowerUp : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player) is false) return;
        if (player.IsInfected) return; //Should zombies be affected or not?
        SoundManager.Instance.PlaySound("LevelUpSound3");
        GameManager.Instance.DespawnAGameObject(gameObject.GetComponent<NetworkObject>().NetworkObjectId);
    }
}
