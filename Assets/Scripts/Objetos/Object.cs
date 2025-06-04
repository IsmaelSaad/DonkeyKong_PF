using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Clase que gestiona el comportamiento de los objetos recolectables en el entorno del juego
public class ObjectEnv
{
    private GameObject itself;        // Referencia al objeto del juego
    private GameManager gameManager; // Referencia al administrador del juego
    private BoxCollider2D boxColl;   // Componente de colisión del objeto
    private int scorePoints;         // Puntos que otorga el objeto al ser recolectado
    private Animator animator;       // Controlador de animaciones del objeto
    private float destroyDelay;      // Tiempo de espera antes de destruir el objeto
    private bool isGotten = false;   // Flag para evitar recolección múltiple
    public bool isHammer = false;    // Indica si el objeto es un martillo especial

    // Constructor: Inicializa todas las propiedades del objeto
    public ObjectEnv(GameObject itself, int scorePoints, Animator animator,
                   float destroyDelay, GameManager gameManager, BoxCollider2D boxColl)
    {
        this.itself = itself;
        this.scorePoints = scorePoints;
        this.animator = animator;
        this.destroyDelay = destroyDelay;
        this.gameManager = gameManager;
        this.boxColl = boxColl;
    }

    // Método llamado cuando el jugador colisiona con el objeto
    public void ObjectOnTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isGotten) // Verifica colisión con jugador y que no haya sido recolectado
        {
            isGotten = true; // Marca como recolectado

            if (!isHammer)
            { // Si no es un martillo
                animator.SetBool("IsScored", true); // Activa animación de recolección
                CoroutineRunner.Instance.StartCoroutine(DestroyAfterAnimation()); // Programa destrucción
            }

            // Efectos de recolección:
            gameManager.AddPoints(scorePoints); // Añade puntos
            MongoDBManager.Instance.playerData.objectsPickedUp++; // Actualiza contador en BD
            // Actualiza visualización de puntos en UI
            GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = gameManager.GetPoints().ToString("D6");
        }
    }

    // Corrutina que destruye el objeto después de la animación
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay); // Espera el tiempo configurado
        Object.Destroy(itself); // Elimina el objeto de la escena
    }
}