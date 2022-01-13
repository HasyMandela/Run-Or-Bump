using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    /*
     Script made by Aslak Aarflot Jønsson
     Fiverr: https://www.fiverr.com/aslakjonsson
         */

    private TreeClass tc;

    public float explosionForce = 500f;
    public float radius = 5f;

    [SerializeField] private GameObject[] typesOfPlank;
    public float secondsBeforeDespawn = 3f;

    public int maxNumPlanks = 5;

    [SerializeField] private GameObject explosionEffect;

    [HideInInspector] public GameObject instantiatedEffect;

    public void Explode(GameObject _brokenObject, GameObject _hittedObject)
    {
        //Adds the particle effect
        instantiatedEffect = Instantiate(explosionEffect, _hittedObject.transform.position, _hittedObject.transform.rotation);

        Instantiate(_brokenObject, _hittedObject.transform.position, _hittedObject.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(_hittedObject.transform.position, radius);

        for (int i = 0; i < maxNumPlanks; i++)
        {
            //Instantiates the planks
            Instantiate(typesOfPlank[Random.Range(0, typesOfPlank.Length)], _hittedObject.transform.position, _hittedObject.transform.rotation);
        }

        foreach (Collider nearbyColliders in colliders)
        {
            Rigidbody rb = nearbyColliders.GetComponent<Rigidbody>();
            if (rb != null)
            {

                rb.AddExplosionForce(explosionForce, _hittedObject.transform.position, radius);
            }
        }

        Destroy(_hittedObject);

        _brokenObject.GetComponent<CleanUpAfterExplosion>().isExploded = true;
    }

    private void Start()
    {
        tc = FindObjectOfType<TreeClass>();
    }
}
