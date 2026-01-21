using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject hostclientPanel;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private TMP_Text IPtext;
  
    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);

    }

    void StartClient()
    {
        SetIP();

        NetworkManager.Singleton.StartClient();
        DeactivateButtons();
    }
    
    void StartHost()
    {
        SetIP();

        NetworkManager.Singleton.StartHost();
        DeactivateButtons();
    }
    private void SetIP()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "10.80.148.138";
        // NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(IPtext.text, 7777);
    }

    void DeactivateButtons()
    {
        hostclientPanel.SetActive(false);
        hostButton.interactable = false;
        clientButton.interactable = false;
    }
}
