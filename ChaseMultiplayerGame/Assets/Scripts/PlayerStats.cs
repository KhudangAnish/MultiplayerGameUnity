using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    private NetworkVariable<float> moveSpeed = new NetworkVariable<float>(15);
    private NetworkVariable<bool> hasProtection = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> isSuperFast = new NetworkVariable<bool>(false);

    private NetworkVariable<bool> hasInvisibility = new NetworkVariable<bool>(false);

    public float GetSpeed() => moveSpeed.Value;
    public bool HasProtection() => hasProtection.Value;
    public bool IsSuperFast() => isSuperFast.Value;

    public bool HasInvisibility() => hasInvisibility.Value;
    public void ModifyStatsSpeed(float newSpeed)
    {
        Debug.Log("Player speed was modified");
        moveSpeed.Value += newSpeed;
    }
    public void ModifyStatsSuperFast(bool isSuperFast)
    {
        Debug.Log("Player protection was modified");
        this.isSuperFast.Value = isSuperFast;
    }
    public void ModifyStatsProtection(bool currentlyHasProtection)
    {
        Debug.Log("Player protection was modified");
        hasProtection.Value = currentlyHasProtection;
    }
    public void ModifyStatsInvisibility(bool currentlyHasInvisibility)
    {
        Debug.Log("Player protection was modified");
        hasInvisibility.Value = currentlyHasInvisibility;
    }
}
