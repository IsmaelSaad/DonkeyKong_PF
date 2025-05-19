using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private bool playerOnSwitch = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerOnSwitch = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerOnSwitch) 
        {
            playerOnSwitch = false;

            if (collision.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }

    }
}
