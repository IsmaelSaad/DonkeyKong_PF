using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador dels barrils d'oli que es poden incendiar
public class OilBarrelController : MonoBehaviour
{
    [SerializeField] Animator fireAnimator; // Animador de l'efecte de foc

    // Quan un objecte entra en la zona de trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprova si és un barril normal o que està rodant
        if (collision.CompareTag("Barrel") || collision.CompareTag("BarrelRolling"))
        {
            // Activa l'efecte de foc
            fireAnimator.gameObject.SetActive(true);
            fireAnimator.enabled = true;
        }
    }

    // Inicialització abans del primer frame
    private void Awake()
    {
        // Assegura que l'efecte de foc està desactivat al començament
        fireAnimator.gameObject.SetActive(false);
    }
}