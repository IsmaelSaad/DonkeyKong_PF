// Controlador del comportament del foc al joc
using UnityEngine;

public class FireController : MonoBehaviour
{
    // Paràmetres configurables des de l'editor
    [SerializeField] private float moveSpeed = 2f;             // Velocitat de moviment
    [SerializeField] private float directionChangeInterval = 3f; // Cada quan canvia de direcció

    // Components i referències
    Fire fire;          // Lògica del foc
    Animator animator;   // Controlador d'animacions
    Rigidbody2D rb;      // Física 2D

    void Start()
    {
        // Obtenim referències als components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Creem la instància del foc amb els paràmetres
        fire = new Fire(animator, moveSpeed, directionChangeInterval, rb);
        fire.EnemyStart();  // Iniciem el comportament del foc
    }

    void Update()
    {
        fire.EnemyUpdate();  // Actualitzem el comportament cada frame
    }
}