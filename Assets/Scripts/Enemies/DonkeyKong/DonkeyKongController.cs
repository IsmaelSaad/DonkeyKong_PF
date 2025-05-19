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
        
        numRollingBarrel = Random.Range(0, 30);
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
    }

    public void LanzarBarrilCritico()
    {
        animator.SetTrigger("ThrowSpecialBarrel");
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

        float randomTime = Random.Range(3f, 1f);
        Invoke("AnimationThrow", randomTime);
    }    

}
