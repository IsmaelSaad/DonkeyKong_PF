using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controla el comportament dels barrils que roden per les escales
public class BarrelRolling : Enemy
{
    private GameObject itself; private Rigidbody2D rb; private CapsuleCollider2D crcColl; // Components bàsics
    private Transform transform; private GameObject barrel; // Referències d'objectes
    private float bounceForce, groundRayDistance = 2.0f; // Paràmetres físics
    private LayerMask groundMaskRolling; // Filtre de capes
    private bool hasGround; // Control de contacte amb terra

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL, DESTROY }; // Estats possibles
    State state = State.MOVEMENT; // Estat inicial

    // Constructor: Configura tot el necessari per al funcionament
    public BarrelRolling(Animator animator, float speed, GameObject itself, Rigidbody2D rb,
                       CapsuleCollider2D crcColl, Transform transform, GameObject barrel,
                       float bounceForce, float groundRayDistance, LayerMask groundMaskRolling)
                       : base(animator, speed)
    {
        this.rb = rb; this.itself = itself; this.crcColl = crcColl;
        this.transform = transform; this.barrel = barrel;
        this.bounceForce = bounceForce; this.groundRayDistance = groundRayDistance;
        this.groundMaskRolling = groundMaskRolling;
    }

    // Actualitza la física del barril cada frame
    public void BarrelRollingFixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMaskRolling);
        hasGround = hit2D.collider != null;
        crcColl.enabled = false; // Desactiva temporalment el collider

        switch (state)
        {
            case State.MOVEMENT:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!hasGround) state = State.FALLING; // Canvia a caiguda si no hi ha terra
                break;

            case State.FALLING:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (hasGround)
                {
                    state = State.BOUNCING;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); // Rebota
                }
                // Destrucció si toca l'últim pis o un barril d'oli
                if (hit2D.collider != null && (hit2D.collider.CompareTag("LastFloor") || hit2D.collider.CompareTag("OilBarrel")))
                {
                    GameObject newBarrel = Object.Instantiate(barrel, transform.position, Quaternion.identity);
                    if (newBarrel != null) newBarrel.GetComponent<BarrelController>().speed *= -1;
                    Object.Destroy(itself);
                    crcColl.enabled = true;
                }
                break;

            case State.BOUNCING:
                speed = Random.Range(-5f, 5f); // Canvia direcció aleatòriament
                if (!hasGround && rb.velocity.y < 0) state = State.BOUNCING_FALL;
                break;

            case State.BOUNCING_FALL:
                if (hasGround) state = State.MOVEMENT; // Torna a moviment normal
                break;

            case State.DESTROY:
                rb.velocity = Vector2.zero; // Atura el moviment
                rb.gravityScale = 0; // Desactiva la gravetat
                break;
        }
    }

    // Gestiona col·lisions amb altres objectes
    public void BarrelRollingOnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OilBarrel")) Object.Destroy(itself); // Destrucció immediata
    }
}