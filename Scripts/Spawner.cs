using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] ThingSpawn;
    [SerializeField] private GameObject[] placeSpawn;
    void Start()
    {
        for (int i = 0; i < placeSpawn.Length; i++){
            int Randomness = Random.Range(0, ThingSpawn.Length);
            Instantiate(ThingSpawn[Randomness], placeSpawn[i].transform.position, Quaternion.identity);
        }
    }
}
