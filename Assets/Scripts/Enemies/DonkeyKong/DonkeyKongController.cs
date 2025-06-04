using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonkeyKongController : MonoBehaviour
{
    [SerializeField] Animator animator; // Referencia al componente Animator para controlar las animaciones
    int numBlueBarrel, numRollingBarrel; // Contadores para determinar el tipo de barril
    int barrelIndex; // Índice del tipo de barril a lanzar
    public GameObject[] Barrels; // Array de prefabs de barriles (normal, azul, rodante)
    public Transform rollingBarrelsSpawn, normalBarrelSpawn; // Puntos de spawn para diferentes barriles
    Vector2 spawnPosition; // Posición donde se instanciará el barril

    // Método para cambiar la animación según el tipo de barril
    public void changeAnimation()
    {
        // 1/30 de probabilidad de lanzar un barril crítico (rodante especial)
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

    // Activa la animación de lanzamiento normal
    public void LanzarBarrilNormal()
    {
        animator.SetTrigger("ThrowBarrel");
    }

    // Activa la animación de lanzamiento especial
    public void LanzarBarrilCritico()
    {
        animator.SetTrigger("ThrowSpecialBarrel");
    }

    // Gestiona la instanciación del barril después de la animación
    void AnimateThrow()
    {
        Vector2 spawnPosition;
        // 1/6 de probabilidad de lanzar un barril azul
        numBlueBarrel = Random.Range(0, 6);

        // Selección del tipo de barril basado en probabilidades
        if (numRollingBarrel == 1)
        {
            // Barriles rodantes especiales (índices 2 y 3)
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
            // Barriles normales o azules
            if (numBlueBarrel == 1)
            {
                barrelIndex = 1; // Barril azul
            }
            else
            {
                barrelIndex = 0; // Barril normal
            }
        }

        // Determina la posición de spawn según el tipo de barril
        if (barrelIndex == 2 || barrelIndex == 3)
        {
            spawnPosition = rollingBarrelsSpawn.position; // Posición para barriles rodantes
        }
        else
        {
            spawnPosition = normalBarrelSpawn.transform.position; // Posición para barriles normales
        }

        // Instancia el barril seleccionado
        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);

        // Programa el próximo lanzamiento con un tiempo aleatorio entre 1 y 3 segundos
        float randomTime = Random.Range(1f, 3f);
        Invoke("changeAnimation", randomTime);
    }
}