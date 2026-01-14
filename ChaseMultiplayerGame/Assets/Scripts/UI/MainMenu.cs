using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject hostclientPanel;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

 
    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DeactivateButtons();
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        DeactivateButtons();
    }

    void DeactivateButtons()
    {
        hostclientPanel.SetActive(false);
        hostButton.interactable = false;
        clientButton.interactable = false;
    }
}
