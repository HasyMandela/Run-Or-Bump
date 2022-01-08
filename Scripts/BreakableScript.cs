using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
