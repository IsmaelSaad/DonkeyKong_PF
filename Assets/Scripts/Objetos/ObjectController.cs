using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controlador d'objectes interactius del joc
public class ObjectController : MonoBehaviour
{
    // Configuració des de l'editor
    [SerializeField] int scorePoints;    // Punts que dóna l'objecte
    [SerializeField] Animator animator;  // Animador de l'objecte
    [SerializeField] float destroyDelay; // Temps abans de destruir l'objecte
    [SerializeField] bool isHammer;      // Indica si és un objecte martell

    // Variables internes
    string objectPoints;
    BoxCollider2D boxColl;      // Col·lider de l'objecte
    GameManager gameManager;    // Referència al GameManager
    ObjectEnv obj;             // Instància de la lògica de l'objecte

    // Inicialització
    void Start()
    {
        // Obtenir components i referències necessàries
        boxColl = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();

        // Crear nova instància de ObjectEnv amb la configuració
        obj = new ObjectEnv(gameObject, scorePoints, animator, destroyDelay, gameManager, boxColl);
    }

    // Gestiona quan un altre objecte entra en contacte (trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        obj.ObjectOnTriggerEnter(collision); // Delegua la lògica a ObjectEnv
    }
}