using UnityEngine;

public class CleanUpAfterExplosion : MonoBehaviour
{
    /*
     Script made by Aslak Aarflot Jønsson
     Fiverr: https://www.fiverr.com/aslakjonsson
         */

    private TreeClass tc;
    private Explosion expl;

    public bool isExploded = false;

    private float countDown;

    private void Start()
    {
        tc = FindObjectOfType<TreeClass>();
        expl = FindObjectOfType<Explosion>();

        countDown = expl.secondsBeforeDespawn;
    }

    private void Update()
    {
        if (isExploded)
        {
            countDown -= Time.deltaTime;

            if (countDown <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
