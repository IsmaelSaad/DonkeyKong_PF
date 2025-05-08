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
    int downStairs;
    

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    BoxCollider2D boxColl;

    bool hasGround = false, hasStairs = false;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL , ONSTAIRS, FALLSTAIRS};
    State state = State.MOVEMENT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.FALLSTAIRS && collision.CompareTag("EscalerasExit")) {
            Debug.Log("asda");
        }


        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hitStairs = Physics2D.Raycast(raycastOriginStairs.position, -raycastOriginStairs.up, stairRayDistance, stairMask);
        hasStairs = hitStairs.collider != null;

         

        
        if (hasStairs && state != State.ONSTAIRS) { // PONER TODOS LOS QUE TENGAN QUE VER CON BARRIL CAYENDO
            downStairs = Random.Range(0, 15);
            if (downStairs == 1)
            {
                if (hitStairs.collider.CompareTag("Escaleras"))
                {
                    state = State.ONSTAIRS;
                }
            }
                    
        }

        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMask);
        hasGround = hit2D.collider != null;

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
                        rb.gravityScale = 0.5f;
                        transform.position = new Vector2(hitStairs.transform.position.x, transform.position.y); 
                        state = State.FALLSTAIRS;
                        animator.SetBool("falling", true);
                        boxColl.enabled = false;
                    }
                }
                else 
                {
                    if (transform.position.x > hitStairs.transform.position.x)
                    {
                        transform.position = new Vector2(hitStairs.transform.position.x, transform.position.y);
                        state = State.FALLSTAIRS;
                        boxColl.enabled = false;
                        animator.SetBool("falling", true);
                    }
                }
                
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("falling", true);
                if (hitStairs.collider.CompareTag("EscalerasEnter") || hitStairs.collider.CompareTag("BarrelEscaleras"))
                {
                    animator.SetBool("falling", false);
                    boxColl.enabled = true;
                    speed *= -1;
                    state = State.MOVEMENT;
                }


                /*if (hitStairs.collider.tag == "EscaleraEnter")
                {
                    state = State.MOVEMENT;
                    animator.SetBool("falling", false);
                    boxColl.enabled = true;
                }
                /*
                if (hitStairs.collider.transform.position.y - 2f > transform.position.y) {
                    
                        if (hasGround)
                        {
                            state = State.MOVEMENT;
                            animator.SetBool("falling", false);
                            boxColl.enabled = true;
                        }
                    }
                } else {
                    boxColl.enabled = false;
                }*/

                //if (!boxColl.enabled) {
                //    if (hasGround) {
                //        return;
                //    }
                //}
                //else if (animator.GetBool("falling")) 
                //{
                //    if (hasGround)
                //    {
                //        boxColl.enabled = false;
                //    }
                //    else 
                //    {
                //        boxColl.enabled = true;
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
                    boxColl.enabled = false;
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
 