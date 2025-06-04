using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador per als interruptors del joc
public class SwitchController : MonoBehaviour
{
    private bool playerOnSwitch = false; // Indica si el jugador està sobre l'interruptor

    // Quan un objecte entra al trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerOnSwitch = true; // Marca que hi ha quelcom sobre l'interruptor
    }

    // Quan un objecte surt del trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerOnSwitch) // Si abans hi havia quelcom sobre l'interruptor
        {
            playerOnSwitch = false; // Actualitza l'estat

            // Si el que surt és el jugador
            if (collision.CompareTag("Player"))
            {
                Destroy(gameObject); // Destrueix l'interruptor
            }
        }
    }
}