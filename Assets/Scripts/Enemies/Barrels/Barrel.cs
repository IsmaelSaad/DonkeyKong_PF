using System.Collections;
using UnityEngine;

public class Barrel : Enemy
{
    private Transform transform; // Components bàsics
    private Rigidbody2D rb; 
    private BoxCollider2D boxColl, playerPointsColl; // Colliders
    private float bounceForce, groundRayDistance = 2.0f, stairRayDistance = 2.0f; // Paràmetres físics
    private Transform actualEscalera; // Referència a escala actual
    private int goDown, barrelPoints = 100, barrelPointsDest = 300; // Valors de joc
    private bool hasGround, hasStairs, exitStairs, genRandom, hasPlayerOn, isPlayerTouchingPoints; // Estats
    private BoxCollider2D detectStair; // Detecció d'escales
    private GameManager gameManager; // Referència global
    public LayerMask groundMask; // Filtre de capes
    public GameObject itself; // Auto-referència

    public enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL, ONSTAIRS, FALLSTAIRS, EXITSTAIRS, DESTROY };
    public State state = State.MOVEMENT;

    // Constructor: Inicialitza tots els components i paràmetres del barril
    public Barrel(Transform transform, Animator animator, float speed, Rigidbody2D rb, BoxCollider2D boxColl,
        BoxCollider2D detectStair, BoxCollider2D playerPointsColl, float bounceForce, float groundRayDistance,
        float stairRayDistance, LayerMask groundMask, GameManager gameManager, GameObject itself) : base(animator, speed)
    {
        this.transform = transform; this.rb = rb; this.boxColl = boxColl;
        this.playerPointsColl = playerPointsColl; this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance; this.detectStair = detectStair;
        this.stairRayDistance = stairRayDistance; this.groundMask = groundMask;
        this.itself = itself; this.gameManager = gameManager;
    }

    // Detecta quan el jugador entra a la zona de punts del barril
    public void PointsOnTriggerEnter2D(Collider2D collision)
    {
        if (playerPointsColl.IsTouching(collision) && !isPlayerTouchingPoints)
            if (collision.CompareTag("Player")) { hasPlayerOn = true; isPlayerTouchingPoints = true; }
    }

    // Detecta quan el jugador surt de la zona de punts
    public void PointsOnTriggerExit2D(Collider2D collision)
    {
        if (!playerPointsColl.IsTouching(collision))
            if (collision.CompareTag("Player")) { hasPlayerOn = false; isPlayerTouchingPoints = false; }
    }

    // Gestiona les col·lisions amb altres objectes (martells, escales)
    public void BarrelOnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hammer") && state != State.DESTROY)
        {
            GameManager.Instance.AddPoints(barrelPointsDest);
            animator.SetBool("destroy", true);
            state = State.DESTROY;
        }
        if (detectStair.IsTouching(collision))
        {
            if (collision.CompareTag("EscalerasExit")) { hasStairs = true; actualEscalera = collision.transform; genRandom = false; }
            if (collision.CompareTag("EscalerasEnter") || collision.CompareTag("BarrelEscaleras")) { hasStairs = false; exitStairs = true; }
        }
    }

    // Gestiona quan el barril deixa de col·lidir amb objectes
    public void BarrelOnTriggerExit2D(Collider2D collision)
    {
        if (!detectStair.IsTouching(collision))
            if (collision.CompareTag("EscalerasExit")) { exitStairs = false; hasStairs = false; }
    }

    // Corrutina que gestiona l'animació i destrucció del barril
    IEnumerator DestroyAfterAnimation()
    {
        boxColl.enabled = false; rb.gravityScale = 0; rb.velocity = Vector3.zero;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.normalizedTime < 1.0f || animator.IsInTransition(0))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool("destroy", false);
        yield return new WaitForSeconds(2f);
        Object.Destroy(itself);
    }

    // Mètode principal que actualitza la física i estats del barril
    public void BarrelFixedUpdate()
    {
        if (state != State.DESTROY)
        {
            if (hasPlayerOn && isPlayerTouchingPoints)
            {
                hasPlayerOn = false;
                MongoDBManager.Instance.playerData.barrelsJumped++;
                gameManager.AddPoints(barrelPoints);
            }

            RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMask);
            hasGround = hit2D.collider != null;

            if (hasStairs && (state != State.FALLSTAIRS || state != State.EXITSTAIRS))
                state = State.ONSTAIRS;

            if (!genRandom && state != State.ONSTAIRS && state != State.FALLSTAIRS && state != State.EXITSTAIRS)
            {
                goDown = Random.Range(0, 20);
                genRandom = true;
            }
        }

        // Màquina d'estats que controla tot el comportament del barril
        switch (state)
        {
            case State.MOVEMENT:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!hasGround) state = State.FALLING;
                break;
            case State.FALLING:
                rb.velocity = new Vector2(speed * 0.5f, rb.velocity.y);
                if (hasGround) { state = State.BOUNCING; rb.velocity = Vector2.zero; rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); }
                break;
            case State.ONSTAIRS:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (detectStair.transform.position.x <= actualEscalera.transform.position.x && goDown == 1)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    transform.position = new Vector2(actualEscalera.transform.position.x, transform.position.y);
                    boxColl.enabled = false;
                    animator.SetBool("falling", true);
                    state = State.FALLSTAIRS;
                }
                if (!hasStairs) state = State.MOVEMENT;
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                transform.position = new Vector2(actualEscalera.transform.position.x, transform.position.y);
                if (exitStairs) { boxColl.enabled = true; state = State.EXITSTAIRS; }
                break;
            case State.EXITSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (hasGround) { speed *= -1; animator.SetBool("falling", false); state = State.MOVEMENT; }
                break;
            case State.BOUNCING:
                if (!hasGround && rb.velocity.y < 0) state = State.BOUNCING_FALL;
                break;
            case State.BOUNCING_FALL:
                boxColl.enabled = true;
                if (hasGround) { speed *= -1; state = State.MOVEMENT; }
                break;
            case State.DESTROY:
                CoroutineRunner.Instance.StartCoroutine(DestroyAfterAnimation());
                break;
        }
    }
}