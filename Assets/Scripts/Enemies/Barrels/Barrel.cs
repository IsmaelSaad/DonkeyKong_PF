using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Player;

public class Barrel : Enemy
{
    private Transform transform;
    private Rigidbody2D rb;
    private BoxCollider2D boxColl, playerPointsColl;
    private float bounceForce;
    private float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    private Transform actualEscalera;
    private int goDown, barrelPoints = 100, barrelPointsDest = 300;
    private bool hasGround = false, hasStairs = false, exitStairs = false, genRandom = false, hasPlayerOn = false, isPlayerTouchingPoints = false;
    private BoxCollider2D detectStair;
    private GameManager gameManager;
    private string playerPoints;
    public LayerMask groundMask;
    public GameObject itself;



    public enum State
    {
        MOVEMENT, FALLING,
        BOUNCING, BOUNCING_FALL,
        ONSTAIRS, FALLSTAIRS,
        EXITSTAIRS, DESTROY
    };
    public State state = State.MOVEMENT;

    public Barrel(Transform transform, Animator animator, float speed, Rigidbody2D rb, BoxCollider2D boxColl, BoxCollider2D detectStair, BoxCollider2D playerPointsColl,  float bounceForce, float groundRayDistance, float stairRayDistance, LayerMask groundMask, GameManager gameManager, GameObject itself) : base(animator, speed)
    {
        this.transform = transform;
        this.rb = rb;
        this.boxColl = boxColl;
        this.playerPointsColl = playerPointsColl;
        this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance;
        this.detectStair = detectStair;
        this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance;
        this.stairRayDistance = stairRayDistance;
        this.groundMask = groundMask;
        this.itself = itself;
        this.gameManager = gameManager;
    }

    public void PointsOnTriggerEnter2D(Collider2D collision)
    {
        if (playerPointsColl.IsTouching(collision) && !isPlayerTouchingPoints)
        {
            if (collision.CompareTag("Player"))
            {
                hasPlayerOn = true;
                isPlayerTouchingPoints = true;
            }
        }
    }

    public void PointsOnTriggerExit2D(Collider2D collision)
    {
        if (!playerPointsColl.IsTouching(collision))
        {
            if (collision.CompareTag("Player"))
            {
                hasPlayerOn = false;
                isPlayerTouchingPoints = false;
            }
        }
    }


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

    //public void BarrelUpdate() 
    //{
    //    Debug.Log(state);
    //}

    IEnumerator DestroyAfterAnimation()
    {
        boxColl.enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;

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


    public void BarrelFixedUpdate()
    {
        if (state != State.DESTROY) {
            if (hasPlayerOn && isPlayerTouchingPoints)
            {
                hasPlayerOn = false;
                gameManager.AddPoints(barrelPoints);
            }

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
        }

        //Debug.Log(state);

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
            case State.DESTROY:
                CoroutineRunner.Instance.StartCoroutine(DestroyAfterAnimation());
                break;

        }
    }

}
