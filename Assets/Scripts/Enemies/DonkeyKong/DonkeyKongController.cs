using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonkeyKongController : MonoBehaviour
{
    [SerializeField] Animator animator; // Referencia al componente Animator para controlar las animaciones
    int numBlueBarrel, numRollingBarrel; // Contadores para determinar el tipo de barril
    int barrelIndex; // �ndice del tipo de barril a lanzar
    public GameObject[] Barrels; // Array de prefabs de barriles (normal, azul, rodante)
    public Transform rollingBarrelsSpawn, normalBarrelSpawn; // Puntos de spawn para diferentes barriles
    Vector2 spawnPosition; // Posici�n donde se instanciar� el barril

    // M�todo para cambiar la animaci�n seg�n el tipo de barril
    public void changeAnimation()
    {
        // 1/30 de probabilidad de lanzar un barril cr�tico (rodante especial)
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

    // Activa la animaci�n de lanzamiento normal
    public void LanzarBarrilNormal()
    {
        animator.SetTrigger("ThrowBarrel");
    }

    // Activa la animaci�n de lanzamiento especial
    public void LanzarBarrilCritico()
    {
        animator.SetTrigger("ThrowSpecialBarrel");
    }

    // Gestiona la instanciaci�n del barril despu�s de la animaci�n
    void AnimateThrow()
    {
        Vector2 spawnPosition;
        // 1/6 de probabilidad de lanzar un barril azul
        numBlueBarrel = Random.Range(0, 6);

        // Selecci�n del tipo de barril basado en probabilidades
        if (numRollingBarrel == 1)
        {
            // Barriles rodantes especiales (�ndices 2 y 3)
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

        // Determina la posici�n de spawn seg�n el tipo de barril
        if (barrelIndex == 2 || barrelIndex == 3)
        {
            spawnPosition = rollingBarrelsSpawn.position; // Posici�n para barriles rodantes
        }
        else
        {
            spawnPosition = normalBarrelSpawn.transform.position; // Posici�n para barriles normales
        }

        // Instancia el barril seleccionado
        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);

        // Programa el pr�ximo lanzamiento con un tiempo aleatorio entre 1 y 3 segundos
        float randomTime = Random.Range(1f, 3f);
        Invoke("changeAnimation", randomTime);
    }
}