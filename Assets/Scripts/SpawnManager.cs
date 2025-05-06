using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] Barrels;
    public Transform rollingBarrelsSpawn;
    int numBlueBarrel, numRollingBarrel;
    int barrelIndex;
    private void Awake()
    { 
        Vector2 spawnPosition;

        spawnPosition = rollingBarrelsSpawn.position;

        barrelIndex = 2;
        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);

    }

    void Start()
    {
        Invoke("spawnBarrel", 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void spawnBarrel()
    {
        Vector2 spawnPosition;
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

        if (barrelIndex == 0 && numRollingBarrel == 1 ) 
        {
            barrelIndex = 3;
        }
        else if (barrelIndex == 1 && numRollingBarrel == 1)
        {
            barrelIndex = 2;
        }

        
         
        if (barrelIndex == 2 || barrelIndex == 3)
        {
            spawnPosition = rollingBarrelsSpawn.position;
        }
        else
        {
            spawnPosition = gameObject.transform.position;
        }
            




        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);

        Invoke("spawnBarrel", randomTime);
    }

    
}
