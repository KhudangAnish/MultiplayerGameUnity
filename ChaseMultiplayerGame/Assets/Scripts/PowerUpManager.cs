using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUpManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> powerupPrefabs;
    [SerializeField] private List<Transform> spawnLocations;


}
