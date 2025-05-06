using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella_object : MonoBehaviour
{
    [SerializeField] int Score;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(obj: gameObject);
        }
    }
}
