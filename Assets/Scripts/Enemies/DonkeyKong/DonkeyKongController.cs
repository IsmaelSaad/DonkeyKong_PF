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
    Vector2 spawnPosition;


    public void changeAnimation()
    {
        
        numRollingBarrel = Random.Range(0, 20);
        if (numRollingBarrel == 1)
        {
            LanzarBarrilCritico();
        }
        else
        {
            LanzarBarrilNormal();
        }
    }


    public void LanzarBarrilNormal()
    {
        animator.SetTrigger("ThrowBarrel");
        //Debug.Log("Lanzo Barril");
        
    }
    public void LanzarBarrilCritico()
    {
        animator.SetTrigger("ThrowSpecialBarrel");
        //Debug.Log("Lanzo Barril critico");
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AnimateThrow()
    {
        Vector2 spawnPosition;
        numBlueBarrel = Random.Range(0, 6);
        


        if (numRollingBarrel == 1) 
        {
            if (barrelIndex == 0)
            {
                barrelIndex = 3;
            }

            else if (barrelIndex == 1)
            {
                barrelIndex = 2;
            }
        } 
        else 
        {
            if (numBlueBarrel == 1)
            {
                barrelIndex = 1;
            }
            else
            {
                barrelIndex = 0;
            }
        }

        if (barrelIndex == 2 || barrelIndex == 3)
        {
            spawnPosition = rollingBarrelsSpawn.position;
        }
        else
        {
            spawnPosition = normalBarrelSpawn.transform.position;
        }




        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);

        float randomTime = Random.Range(3f, 10f);
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
