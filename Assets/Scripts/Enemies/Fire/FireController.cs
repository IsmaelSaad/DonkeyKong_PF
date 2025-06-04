// Controlador del comportament del foc al joc
using UnityEngine;

public class FireController : MonoBehaviour
{
    // Par�metres configurables des de l'editor
    [SerializeField] private float moveSpeed = 2f;             // Velocitat de moviment
    [SerializeField] private float directionChangeInterval = 3f; // Cada quan canvia de direcci�

    // Components i refer�ncies
    Fire fire;          // L�gica del foc
    Animator animator;   // Controlador d'animacions
    Rigidbody2D rb;      // F�sica 2D

    void Start()
    {
        // Obtenim refer�ncies als components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Creem la inst�ncia del foc amb els par�metres
        fire = new Fire(animator, moveSpeed, directionChangeInterval, rb);
        fire.EnemyStart();  // Iniciem el comportament del foc
    }

    void Update()
    {
        fire.EnemyUpdate();  // Actualitzem el comportament cada frame
    }
}