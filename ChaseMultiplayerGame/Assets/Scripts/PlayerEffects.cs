using System.Collections.Generic;
using UnityEngine;



public enum Effect
{
    Speed,
    Protection
}


[System.Serializable]
public class AppliedEffect
{
    public Effect effect;
    public float effectTime = 5f;
    private float currentTime = 0f;
    public PlayerStats playerStats;


    public AppliedEffect(Effect effect, float effectTime, PlayerStats playerStats)
    {
        this.effect = effect;
        this.effectTime = effectTime;
        this.playerStats = playerStats;
    }
    public bool IsFinished()
    {
        if (currentTime < effectTime) return false;
        return true;
    }
    public void ApplyEffect()
    {
        switch (effect)
        {
            case Effect.Speed:
                playerStats.ModifyStatsSpeedServerRpc(100);
                break;
            case Effect.Protection:
                playerStats.ModifyStatsProtectionServerRpc(true);
                break;
        }
    }
    public void RemoveEffect()
    {
        switch (effect)
        {
            case Effect.Speed:
                playerStats.ModifyStatsSpeedServerRpc(-100);
                break;
            case Effect.Protection:
                playerStats.ModifyStatsProtectionServerRpc(false);
                break;
        }
    }

    public void Update()
    {
        currentTime += Time.deltaTime;
    }

}
public class PlayerEffects : MonoBehaviour
{
    public List<AppliedEffect> appliedEffects = new();
    [SerializeField] private PlayerStats playerStats;


    public void AddEffect(AppliedEffect appliedEffect)
    {
        appliedEffect.ApplyEffect();
        appliedEffects.Add(appliedEffect);
    }


    private void Update()
    {
        if (appliedEffects.Count == 0) return;

        List<AppliedEffect> effectsToRemove = new();

        foreach (AppliedEffect appliedEffects in appliedEffects)
        {
            appliedEffects.Update();
            if(appliedEffects.IsFinished())
            {
                effectsToRemove.Add(appliedEffects);
            }
        }
        for (int i = 0; i < effectsToRemove.Count; i++)
        {
            if (appliedEffects.FindAll(e => e.effect == Effect.Speed).Count <= 1)
            {
                effectsToRemove[i].RemoveEffect();
            }
            else if (appliedEffects.FindAll(e => e.effect == Effect.Protection).Count <= 1)
            {
                effectsToRemove[i].RemoveEffect();
            }
            appliedEffects.Remove(effectsToRemove[i]);
        }
    }

}
