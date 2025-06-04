using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador principal dels barrils que delega la l�gica a la classe Barrel
public class BarrelController : MonoBehaviour
{
    [SerializeField] Animator animator; // Control d'animacions
    [SerializeField] public float speed; // Velocitat base del barril  
    [SerializeField] BoxCollider2D detectStair, playerPointsColl; // Colliders d'escalas i punts
    [SerializeField] float bounceForce, groundRayDistance = 2.0f, stairRayDistance = 2.0f; // Par�metres f�sics
    [SerializeField] LayerMask groundMask; // Filtre de capes per terra

    private GameObject itself; private GameManager gameManager; // Refer�ncies essentials
    private Rigidbody2D rb; private BoxCollider2D boxColl; // Components Unity
    private Barrel barrel; // Inst�ncia de la l�gica principal

    void Start()
    {
        itself = gameObject;
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        barrel = new Barrel(transform, animator, speed, rb, boxColl, detectStair,
                          playerPointsColl, bounceForce, groundRayDistance,
                          stairRayDistance, groundMask, gameManager, itself);
    }

    void FixedUpdate() => barrel.BarrelFixedUpdate(); // Delegar f�sica

    private void OnTriggerEnter2D(Collider2D collision)
    {
        barrel.PointsOnTriggerEnter2D(collision); // Gestionar punts
        barrel.BarrelOnTriggerEnter2D(collision); // Gestionar col�lisions
        if (collision.CompareTag("OilBarrel")) Destroy(gameObject); // Destrucci� amb oilBarrel
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        barrel.BarrelOnTriggerExit2D(collision); // Sortida col�lisions
        barrel.PointsOnTriggerExit2D(collision); // Sortida zona punts
    }
}