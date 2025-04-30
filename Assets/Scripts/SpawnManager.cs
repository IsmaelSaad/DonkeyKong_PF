using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] Barrels;
    int num;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            num = Random.Range(0, 6);
            int barrelIndex;

            if (num == 1)
            {
                barrelIndex = 1;
            }
            else
            {
                barrelIndex = 0;
            }

            Instantiate(Barrels[barrelIndex], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Barrels[0].transform.rotation);
        }
    }
}
