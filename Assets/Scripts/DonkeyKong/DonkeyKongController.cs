using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonkeyKongController : MonoBehaviour
{
    [SerializeField] Animator animator;
    int numBlueBarrel, numRollingBarrel;
    int barrelIndex;
    public GameObject[] Barrels;
    public Transform rollingBarrelsSpawn, normalBarrelSpawn;
    public void LanzarBarrilNormal()
    {
        Debug.Log("Lanzo Barril");
    }
    public void LanzarBarrilCritico()
    {
        Debug.Log("Lanzo Barril Especial");
    }

    void Start()
    {
        Invoke("AnimateThrow", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AnimateThrow()
    {
        numBlueBarrel = Random.Range(0, 6);
        numRollingBarrel = Random.Range(0, 20);


        switch (numBlueBarrel)
        {
            case 1:
               barrelIndex = 1;
               animator.SetTrigger("ThrowBarrel");
               break;
            default:
               barrelIndex = 0;
               animator.SetTrigger("ThrowBarrel");
               break;
        }

        if (barrelIndex == 0 && numRollingBarrel == 1)
        {    
            barrelIndex = 3;    
            animator.SetTrigger("ThrowSpecialBarrel");
        }
            
        else if (barrelIndex == 1 && numRollingBarrel == 1)
        {
            barrelIndex = 2;
            animator.SetTrigger("ThrowSpecialBarrel");
        }

        Instantiate(Barrels[barrelIndex], normalBarrelSpawn.transform.position, Barrels[0].transform.rotation);

        float randomTime = Random.Range(1.5f, 3f);
        Invoke("AnimationThrow", randomTime);

    }

    /*void spawnBarrel()
    {
        
        float randomTime = Random.Range(1.5f, 3f);
        numBlueBarrel = Random.Range(0, 6);
        numRollingBarrel = Random.Range(0, 20);


        switch (numBlueBarrel)
        {
            case 1:
                barrelIndex = 1;
                break;
            default:
                barrelIndex = 0;
                break;
        }

        if (barrelIndex == 0 && numRollingBarrel == 1)
        {
            barrelIndex = 3;
        }
        else if (barrelIndex == 1 && numRollingBarrel == 1)
        {
            barrelIndex = 2;
        }

        Invoke("spawnBarrel", randomTime);
    }*/
}
