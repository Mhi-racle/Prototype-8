using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public RoadSpawnManager roadSpawnManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            roadSpawnManager.SpawnTriggerEntered();
            Debug.Log("Spawned");
        }
    }
}
