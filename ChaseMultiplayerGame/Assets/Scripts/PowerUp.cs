using Unity.Netcode;
using UnityEngine;

public class PowerUp : NetworkBehaviour
{
    [SerializeField] private Effect effect;
    [SerializeField] private float effectTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player) is false) return;
        if (player.IsInfected) return; //Should zombies be affected or not?
        SoundManager.Instance.PlaySound("LevelUpSound3");
        //player.GetComponent<PlayerEffects>().AddEffect(new AppliedEffect(effect, effectTime, player.GetComponent<PlayerStats>()));
        ApplyPowerUpRpc(player.GetComponent<NetworkObject>().NetworkObjectId);
        GameManager.Instance.DespawnAGameObject(gameObject.GetComponent<NetworkObject>().NetworkObjectId);
    }

    [Rpc(SendTo.Server)]
    private void ApplyPowerUpRpc(ulong despawnGameObject)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(despawnGameObject, out NetworkObject n);
        n.GetComponent<PlayerEffects>().AddEffect(new AppliedEffect(effect, effectTime, n.GetComponent<PlayerStats>()));
    }
}
