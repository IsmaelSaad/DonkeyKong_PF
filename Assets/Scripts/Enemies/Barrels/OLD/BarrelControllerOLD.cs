using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControllerOLD : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] public float speed;
    [SerializeField] BoxCollider2D detectStair;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask;

    int goDown;

    Transform actualEscalera;

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    BoxCollider2D boxColl;

    bool hasGround = false, hasStairs = false, exitStairs = false, genRandom = false;

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
                genRandom = false;
            }
            if (collision.CompareTag("EscalerasEnter") || collision.CompareTag("BarrelEscaleras"))
            {
                hasStairs = false;
                exitStairs = true;
            }
        }

        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!detectStair.IsTouching(collision))
        {
            if (collision.CompareTag("EscalerasExit"))
            {
                exitStairs = false;
                hasStairs = false;
            }
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

        if (hasStairs && (state != State.FALLSTAIRS || state != State.EXITSTAIRS)) {
            state = State.ONSTAIRS;
        }

        //Debug.Log(genRandom);
        //Debug.Log(state);

        if (!genRandom && state != State.ONSTAIRS && state != State.FALLSTAIRS && state != State.EXITSTAIRS) {
            goDown = Random.Range(0, 15);
            genRandom = true;
        }

        //Debug.Log(goDown);
        //Debug.Log(hasStairs);

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
                break;
            case State.ONSTAIRS:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (detectStair.transform.position.x <= actualEscalera.transform.position.x) {
                    if (goDown == 1) {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        transform.position = new Vector2(actualEscalera.transform.position.x, transform.position.y);
                        boxColl.enabled = false;
                        animator.SetBool("falling", true);
                        state = State.FALLSTAIRS;
                    }
                }
                if (!hasStairs) state = State.MOVEMENT;
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                transform.position = new Vector2(actualEscalera.transform.position.x, transform.position.y);
                if (exitStairs)
                {
                    boxColl.enabled = true;
                    state = State.EXITSTAIRS;
                }
                break;
            case State.EXITSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (hasGround)
                {
                    speed *= -1;
                    animator.SetBool("falling", false);
                    state = State.MOVEMENT;
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
