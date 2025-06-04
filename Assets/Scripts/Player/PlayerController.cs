using UnityEngine;
using UnityEngine.InputSystem;

// Controlador principal del jugador (Mario)
public class PlayerController : MonoBehaviour
{
    // Paràmetres configurables des de l'editor
    [SerializeField] LayerMask floorLayer, stairsLayer;      // Capes per a detecció de sòl i escales
    [SerializeField] Transform raycastOrigin;               // Origen del raig de detecció
    [SerializeField] InputActionAsset inputActionMapping;   // Configuració de controls
    [SerializeField] float speed = 5f;                     // Velocitat de moviment
    [SerializeField] float jumpSpeed = 10f;                // Força del salt
    [SerializeField] Collider2D detectFloor;               // Col·lider per detectar sòl
    [SerializeField] Collider2D capsuleCollider;           // Col·lider principal del jugador
    [SerializeField] Animator hammerAnimator;               // Animador del martell
    [SerializeField] SpriteRenderer hammerRenderer;         // Render del martell
    [SerializeField] BoxCollider2D hammerColl;             // Col·lider del martell

    // Components interns
    Rigidbody2D rb;        // Component físic
    Animator animator;     // Animador del jugador

    // Instància de la classe Player que conté la lògica
    public Player mario;

    // Inicialització abans del primer frame
    private void Awake()
    {
        // Obtenir components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Crear nova instància de Player amb tots els paràmetres necessaris
        mario = new Player(transform, raycastOrigin, speed, jumpSpeed, rb, animator,
                         inputActionMapping, floorLayer, capsuleCollider, hammerAnimator,
                         hammerRenderer, hammerColl);

        mario.WakePlayer(); // Activar controls del jugador
    }

    // Inicialització al començament
    void Start()
    {
        mario.StartPlayer(); // Iniciar estat del jugador
    }

    // Actualització cada frame
    void Update()
    {
        mario.UpdatePlayer(); // Actualitzar lògica del jugador
    }

    // Gestiona col·lisions físiques
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mario.BarrilCollisionEnter(collision); // Col·lisió amb barrils
        mario.SueloCollisionEnter(collision);  // Col·lisió amb sòl que canvia direcció
    }

    // Gestiona triggers (col·lisions sense física)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mario.SwitchCollisionEnter(collision);    // Interruptors
        mario.EventCollisionEnter(collision);     // Esdeveniments
        mario.BarrilTriggerEnter(collision);      // Barrils (trigger)
        mario.HammerCollisionEnter(collision);    // Martell

        // Si està tocant terra, gestiona escales
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

    // Mètode per obtenir la instància de Mario
    public Player GetMario()
    {
        return mario;
    }
}