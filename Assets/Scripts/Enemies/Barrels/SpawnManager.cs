using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gestiona la generació de barrils al joc
public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] Barrels; // Array de prefabs de barrils (normal, blau, rodó)
    [SerializeField] Transform rollingBarrelsSpawn; // Punt de spawn pels barrils rodons

    private int barrelIndex; // Índex del tipus de barril a generar

    private void Awake()
    {
        SpawnInitialBarrel(); // Genera un barril inicial al començament
    }

    // Genera el barril inicial en la posició designada
    private void SpawnInitialBarrel()
    {
        Vector2 spawnPosition = rollingBarrelsSpawn.position;
        barrelIndex = 2; // Índex per barril rodó (segons l'array)
        Instantiate(Barrels[barrelIndex], spawnPosition, Barrels[0].transform.rotation);
    }

    // Mètode per generar barrils durant el joc (pendent d'implementar)
    public void SpawnBarrel(Vector2 position, int typeIndex)
    {
        Instantiate(Barrels[typeIndex], position, Quaternion.identity);
    }
}