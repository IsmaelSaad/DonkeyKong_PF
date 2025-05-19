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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
