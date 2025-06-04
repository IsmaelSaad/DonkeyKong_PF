using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controlador d'objectes interactius del joc
public class ObjectController : MonoBehaviour
{
    // Configuraci� des de l'editor
    [SerializeField] int scorePoints;    // Punts que d�na l'objecte
    [SerializeField] Animator animator;  // Animador de l'objecte
    [SerializeField] float destroyDelay; // Temps abans de destruir l'objecte
    [SerializeField] bool isHammer;      // Indica si �s un objecte martell

    // Variables internes
    string objectPoints;
    BoxCollider2D boxColl;      // Col�lider de l'objecte
    GameManager gameManager;    // Refer�ncia al GameManager
    ObjectEnv obj;             // Inst�ncia de la l�gica de l'objecte

    // Inicialitzaci�
    void Start()
    {
        // Obtenir components i refer�ncies necess�ries
        boxColl = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();

        // Crear nova inst�ncia de ObjectEnv amb la configuraci�
        obj = new ObjectEnv(gameObject, scorePoints, animator, destroyDelay, gameManager, boxColl);
    }

    // Gestiona quan un altre objecte entra en contacte (trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        obj.ObjectOnTriggerEnter(collision); // Delegua la l�gica a ObjectEnv
    }
}