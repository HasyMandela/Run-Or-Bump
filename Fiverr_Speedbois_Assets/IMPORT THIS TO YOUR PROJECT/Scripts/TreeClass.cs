using UnityEngine;

public class TreeClass : MonoBehaviour
{
    /*
     Script made by Aslak Aarflot Jønsson
     Fiverr: https://www.fiverr.com/aslakjonsson
         */

    public GameObject brokenObject;
    private Explosion expl;



    private void Start()
    {
        expl = FindObjectOfType<Explosion>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            expl.Explode(brokenObject, transform.parent.gameObject);
        }
    }
}
