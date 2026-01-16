using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUpManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> powerupPrefabs;
    [SerializeField] private List<PowerUpLocation> spawnLocations;


    private float spawnRate = 5f;
    private float currentSpawnTime = 0;



    private void Update()
    {
        if (!IsServer || !IsHost) return; //Only run this on the server side

        currentSpawnTime += Time.deltaTime;

        if(currentSpawnTime >= spawnRate)
        {
            currentSpawnTime = 0;
            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        List<PowerUpLocation> emptyLocations = spawnLocations.FindAll(s => s.powerUp == null);

        int locationIndex = Random.Range(0, emptyLocations.Count);
        int powerUpIndex = Random.Range(0, powerupPrefabs.Count);

        GameObject spawnedPowerup = Instantiate(powerupPrefabs[powerUpIndex], spawnLocations[locationIndex].SpawnPosition.position, Quaternion.identity);
        spawnLocations[locationIndex].powerUp = spawnedPowerup.GetComponent<PowerUp>(); //We don't really need to spawn this, we could have spawned only the GameObject if we wanted to
        spawnedPowerup.GetComponent<NetworkObject>().Spawn(true);

      //  var spawnedPowerup = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(powerupPrefabs[powerUpIndex].GetComponent<NetworkObject>(), position: spawnLocations[locationIndex].SpawnPosition.position); //You need a NetworkObject as parameter? But how can you have an object that you are supposed to spawn?
      //  spawnLocations[locationIndex].powerUp = spawnedPowerup.GetComponent<PowerUp>(); //We don't really need to spawn this, we could have spawned only the GameObject if we wanted to

    }
}
