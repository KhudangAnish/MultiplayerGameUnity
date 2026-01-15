using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



//Maybe replace this in the future
//public enum SoundEffect
//{
//    Footstep,
//    Infected,
//    WindPowerup,
//    SpeedPowerUp,
//    ProtectionPowerUp
//}
[System.Serializable]
public class Audio
{
    public string AudioName => AudioClip.name;
    public AudioClip AudioClip;
    public AudioSource AudioSource;
}
public class SoundManager : NetworkBehaviour
{
   // [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Audio> audios;


    private static SoundManager instance;
    public static SoundManager Instance => instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundEffectName)
    {
        PlaySoundServerRpc(soundEffectName);
    }

    [Rpc(SendTo.Server)]
    private void PlaySoundServerRpc(string soundEffectName)
    {
        PlaySoundEveryoneClientRpc(soundEffectName);
    }

    [ClientRpc]
    private void PlaySoundEveryoneClientRpc(string soundEffectName)
    {
        foreach (var audio in audios)
        {
            Debug.Log(audio.AudioName);
        }
        Audio audioClip = audios.Find(a => a.AudioName == soundEffectName);
        audioClip.AudioSource.PlayOneShot(audioClip.AudioClip);
    }
}
