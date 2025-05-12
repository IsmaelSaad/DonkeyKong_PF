using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR;

public class BarrelController : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] public float speed;
    [SerializeField] BoxCollider2D detectStair;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask;

    Transform actualEscalera;

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    BoxCollider2D boxColl;

    bool hasGround = false, hasStairs = false;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL, ONSTAIRS, FALLSTAIRS, EXITSTAIRS };
    State state = State.MOVEMENT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponentInChildren<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (detectStair.IsTouching(collision))
        {
            if (collision.CompareTag("EscalerasExit"))
            {
                hasStairs = true;
                actualEscalera = collision.transform;
            }
            if (collision.CompareTag("EscalerasEnter")) 
            {
                hasStairs = false;
            }
        }

        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMask);
        hasGround = hit2D.collider != null;

        if (hasStairs && state != State.FALLSTAIRS) {
            state = State.ONSTAIRS;
        }

        Debug.Log(state);

        switch (state)
        {
            case State.MOVEMENT:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!hasGround)
                {
                    state = State.FALLING;
                }
                break;
            case State.FALLING:

                rb.velocity = new Vector2(speed * 0.5f, rb.velocity.y);
                if (hasGround)
                {
                    state = State.BOUNCING;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
                else
                {
                    boxColl.enabled = false;
                }
                break;
            case State.ONSTAIRS:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (detectStair.transform.position.x <= actualEscalera.transform.position.x) {
                    transform.position = new Vector2(actualEscalera.transform.position.x, transform.position.y);
                    speed = 0;
                    boxColl.enabled = false;
                    state = State.FALLSTAIRS;
                }
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (!hasStairs)
                {
                    boxColl.enabled = true;
                }
                break;
            case State.BOUNCING:

                if (!hasGround)
                {
                    if (rb.velocity.y < 0)
                    {
                        state = State.BOUNCING_FALL;
                    }

                }
                break;
            case State.BOUNCING_FALL:
                boxColl.enabled = true;
                if (hasGround)
                {
                    speed *= -1;
                    state = State.MOVEMENT;
                }
                break;

        }

    }





}
