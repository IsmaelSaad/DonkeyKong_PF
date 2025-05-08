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
    [SerializeField] Transform raycastOriginStairs;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask, stairMask;
    

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    CircleCollider2D crcColl;

    bool hasGround = false, hasStairs = false;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL , ONSTAIRS, FALLSTAIRS};
    State state = State.MOVEMENT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hitStairs = Physics2D.Raycast(raycastOriginStairs.position, -raycastOriginStairs.up, stairRayDistance, stairMask);
        hasStairs = hitStairs.collider != null;

        Debug.DrawLine(raycastOriginStairs.position, raycastOriginStairs.position - raycastOriginStairs.up * stairRayDistance, Color.red);

        if (hasStairs) {
            if (hitStairs.collider.CompareTag("Escaleras")) {
                state = State.ONSTAIRS;
            }
        }


        Debug.Log(state);

        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMask);
        hasGround = hit2D.collider != null;

        Debug.Log();

        switch (state)
        {
            case State.MOVEMENT:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!hasGround)
                {
                    state = State.FALLING;
                }
                break;
            case State.ONSTAIRS:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (rb.velocity.x < 0)
                {
                    if (transform.position.x < hitStairs.transform.position.x)
                    {
                        state = State.FALLSTAIRS;
                        animator.SetBool("falling", true);
                        crcColl.enabled = false;
                    }
                }
                else 
                {
                    if (transform.position.x > hitStairs.transform.position.x)
                    {
                        state = State.FALLSTAIRS;
                        animator.SetBool("falling", true);
                        crcColl.enabled = false;
                    }
                }
                
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (hitStairs.collider.transform.position.y > transform.position.y) {

                    if (hasGround) {
                        state = State.MOVEMENT;
                        animator.SetBool("falling", false);
                        crcColl.enabled = true;
                    }
                } else {
                    crcColl.enabled = false;
                }

                //if (!crcColl.enabled) {
                //    if (hasGround) {
                //        return;
                //    }
                //}
                //else if (animator.GetBool("falling")) 
                //{
                //    if (hasGround)
                //    {
                //        crcColl.enabled = false;
                //    }
                //    else 
                //    {
                //        crcColl.enabled = true;
                //        state = State.MOVEMENT;
                //        animator.SetBool("falling", false);
                //    }
                //}
                break;
            case State.FALLING:
                
                rb.velocity = new Vector2(speed * 0.5f , rb.velocity.y);
                if (hasGround)
                {
                    state = State.BOUNCING;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
                else { 
                    crcColl.enabled = false;
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
                crcColl.enabled = true;
                if (hasGround)
                {
                    speed *= -1;
                    state = State.MOVEMENT;
                }
                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }



}
 