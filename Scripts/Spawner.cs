using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] placeSpawners;
    [SerializeField] private GameObject[] ThingSpawn;
    void Update()
    {
        for (int i = 0; i < placeSpawners.Length; i++){
            placeSpawners[i] = ThingSpawn[i];
        }
    }
}
