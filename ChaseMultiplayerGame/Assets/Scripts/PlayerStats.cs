using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    private NetworkVariable<float> moveSpeed = new NetworkVariable<float>(15);
    private NetworkVariable<bool> hasProtection = new NetworkVariable<bool>(false);
   // private NetworkVariable<bool> HasProtection = new NetworkVariable<bool>();




    public float GetSpeed() => moveSpeed.Value;
    public bool HasProtection() => hasProtection.Value;



    [ServerRpc]
    public void ModifyStatsSpeedServerRpc(float newSpeed)
    {
        Debug.Log("Player speed was modified");
        moveSpeed.Value += newSpeed;
    }

    [ServerRpc]
    public void ModifyStatsProtectionServerRpc(bool currentlyHasProtection)
    {
        Debug.Log("Player protection was modified");
        hasProtection.Value = currentlyHasProtection;
    }
}
