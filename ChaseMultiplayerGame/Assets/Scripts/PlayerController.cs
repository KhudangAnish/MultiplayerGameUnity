using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private GameObject nonInfected;
   [SerializeField] private GameObject infected;

    private bool isInfected = false;

    [SerializeField] private PlayerStats playerstats;
    [SerializeField] private GameObject normalBody;
    [SerializeField] private GameObject playerUsernameGameObject;
    [SerializeField] private GameObject protectionBubble;

    public bool IsInfected => isInfected;
    // read from other clients
    private NetworkVariable<FixedString64Bytes> PlayerUsername
        = new NetworkVariable<FixedString64Bytes>("UnKnown",
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private NetworkVariable<int> ProfileIndex
      = new NetworkVariable<int>(0,
          NetworkVariableReadPermission.Everyone,
          NetworkVariableWritePermission.Owner);

    [SerializeField] Transform profileHoder;

    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] Image profileImage;
    public override void OnNetworkSpawn()
    {
        PlayerUsername.OnValueChanged += SetUsernameText;
        SetUsernameText(default, PlayerUsername.Value);

        if (IsOwner) CameraController.Instance.InitializeCamera(transform);
    }
    private void Start()
    {
        ProfileIndex.OnValueChanged += SetUserProfileIndex;
        SetUserProfileIndex(default, ProfileIndex.Value);
        Reset();
        playerstats = GetComponent<PlayerStats>();
    }

    public void SetMyName()
    {
        SetUsernameText(default, PlayerUsername.Value);
        if (IsOwner)
        {
            PlayerUsername.Value = GameManager.Instance.GetUsername();
        }
    }

    public void SetMyProfile()
    {
        SetUserProfileIndex(default, ProfileIndex.Value);
        if (IsOwner)
        {
            ProfileIndex.Value = GameManager.Instance.GetProfileIndex();
        }
    }

    public void GetInfected()
    {
        isInfected = true;
        nonInfected.SetActive(false);
        infected.SetActive(true);
    }

    private void Update()
    {
        profileHoder.forward = Camera.main.transform.forward;
        if (playerstats.HasInvisibility())
        {
            normalBody.SetActive(false);
            playerUsernameGameObject.SetActive(false);
        }
        else
        {
            normalBody.SetActive(true);
            playerUsernameGameObject.SetActive(true);
        }
        if (playerstats.HasProtection())
            protectionBubble.SetActive(true);
        else
            protectionBubble.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isInfected is false) return;
        if (collision.collider.GetComponent<PlayerStats>().HasProtection()) return; //Can't do anything when he is protected
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

    public void SetUsernameText(FixedString64Bytes prev, FixedString64Bytes aName)
    {
        usernameText.text = aName.ToString();
    }
    public void SetUserProfileIndex(int prev, int aIndex)
    {
        profileImage.sprite = GameManager.Instance.profileSprites[aIndex];
    }
}
