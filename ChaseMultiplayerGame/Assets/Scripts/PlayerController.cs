using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
   [SerializeField] private Material nonInfectedMaterial;
   [SerializeField] private Material infectedMaterial;

    private bool isInfected = false;

    public bool IsInfected => isInfected;

    private void Awake()
    {
       // GetComponentInChildren<MeshRenderer>().material = nonInfectedMaterial;
    }



    public void GetInfected()
    {
        isInfected = true;
        GetComponentInChildren<MeshRenderer>().material = infectedMaterial;
    }
}
