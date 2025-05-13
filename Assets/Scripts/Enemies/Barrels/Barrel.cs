using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR;

public class Barrel : Enemy
{
    private Transform transform;
    private Rigidbody2D rb;
    private BoxCollider2D boxColl;
    private float bounceForce;
    private float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    private Transform actualEscalera;
    private bool hasGround = false, hasStairs = false, exitStairs = false, genRandom = false;
    private BoxCollider2D detectStair;
    public LayerMask groundMask;

    private int goDown;

    public enum State
    {
        MOVEMENT, FALLING,
        BOUNCING, BOUNCING_FALL,
        ONSTAIRS, FALLSTAIRS,
        EXITSTAIRS
    };
    public State state = State.MOVEMENT;


    public Barrel(Transform transform, Animator animator, float speed, Rigidbody2D rb, BoxCollider2D boxColl, BoxCollider2D detectStair, float bounceForce, float groundRayDistance, float stairRayDistance, LayerMask groundMask) : base(animator, speed) {
        this.transform = transform;
        this.rb = rb;
        this.boxColl = boxColl;
        this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance;
        this.detectStair = detectStair;
        this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance;
        this.stairRayDistance = stairRayDistance;
        this.groundMask = groundMask;
    }

    public void BarrelOnTriggerEnter2D(Collider2D collision)
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
    }

    public void BarrelOnTriggerExit2D(Collider2D collision)
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

    public void BarrelUpdate() 
    {
        Debug.Log(state);
    }

    public void BarrelFixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMask);
        hasGround = hit2D.collider != null;

        if (hasStairs && (state != State.FALLSTAIRS || state != State.EXITSTAIRS))
        {
            state = State.ONSTAIRS;
        }

        if (!genRandom && state != State.ONSTAIRS && state != State.FALLSTAIRS && state != State.EXITSTAIRS)
        {
            goDown = Random.Range(0, 20);
            genRandom = true;
        }

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
                if (detectStair.transform.position.x <= actualEscalera.transform.position.x)
                {
                    if (goDown == 1)
                    {
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
