using UnityEngine;
using UnityEngine.InputSystem;

// Controlador principal del jugador (Mario)
public class PlayerController : MonoBehaviour
{
    // Par�metres configurables des de l'editor
    [SerializeField] LayerMask floorLayer, stairsLayer;      // Capes per a detecci� de s�l i escales
    [SerializeField] Transform raycastOrigin;               // Origen del raig de detecci�
    [SerializeField] InputActionAsset inputActionMapping;   // Configuraci� de controls
    [SerializeField] float speed = 5f;                     // Velocitat de moviment
    [SerializeField] float jumpSpeed = 10f;                // For�a del salt
    [SerializeField] Collider2D detectFloor;               // Col�lider per detectar s�l
    [SerializeField] Collider2D capsuleCollider;           // Col�lider principal del jugador
    [SerializeField] Animator hammerAnimator;               // Animador del martell
    [SerializeField] SpriteRenderer hammerRenderer;         // Render del martell
    [SerializeField] BoxCollider2D hammerColl;             // Col�lider del martell

    // Components interns
    Rigidbody2D rb;        // Component f�sic
    Animator animator;     // Animador del jugador

    // Inst�ncia de la classe Player que cont� la l�gica
    public Player mario;

    // Inicialitzaci� abans del primer frame
    private void Awake()
    {
        // Obtenir components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Crear nova inst�ncia de Player amb tots els par�metres necessaris
        mario = new Player(transform, raycastOrigin, speed, jumpSpeed, rb, animator,
                         inputActionMapping, floorLayer, capsuleCollider, hammerAnimator,
                         hammerRenderer, hammerColl);

        mario.WakePlayer(); // Activar controls del jugador
    }

    // Inicialitzaci� al comen�ament
    void Start()
    {
        mario.StartPlayer(); // Iniciar estat del jugador
    }

    // Actualitzaci� cada frame
    void Update()
    {
        mario.UpdatePlayer(); // Actualitzar l�gica del jugador
    }

    // Gestiona col�lisions f�siques
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mario.BarrilCollisionEnter(collision); // Col�lisi� amb barrils
        mario.SueloCollisionEnter(collision);  // Col�lisi� amb s�l que canvia direcci�
    }

    // Gestiona triggers (col�lisions sense f�sica)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mario.SwitchCollisionEnter(collision);    // Interruptors
        mario.EventCollisionEnter(collision);     // Esdeveniments
        mario.BarrilTriggerEnter(collision);      // Barrils (trigger)
        mario.HammerCollisionEnter(collision);    // Martell

        // Si est� tocant terra, gestiona escales
        if (detectFloor.IsTouching(collision))
        {
            mario.EscalerasCollisionEnter(collision);
        }
    }

    // Gestiona sortida de triggers
    private void OnTriggerExit2D(Collider2D collision)
    {
        mario.SwitchCollisionExit(collision);    // Sortida d'interruptor
        mario.EscalerasCollisionExit(collision); // Sortida d'escala
    }

    // M�tode per obtenir la inst�ncia de Mario
    public Player GetMario()
    {
        return mario;
    }
}