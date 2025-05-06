using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrelRollingController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject Barrel;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f;
    [SerializeField] LayerMask groundMaskRolling;
    

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    CapsuleCollider2D crcColl;

    bool hasGround = false;
    bool lastFloor = false;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL };
    State state = State.MOVEMENT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CapsuleCollider2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position, Vector2.down, groundRayDistance, groundMaskRolling);
        hasGround = hit2D.collider != null;
        crcColl.enabled = false;
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
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (hasGround)
                {
                    state = State.BOUNCING;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
                if (hit2D.collider.CompareTag("LastFloor"))
                {
                    Instantiate(Barrel, gameObject.transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    crcColl.enabled = true;
                    speed *= -1;
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
                
                if (hasGround)
                {
                    state = State.MOVEMENT;
                }
                
                break;

        }


    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("LastFloor"))
        {
            lastFloor = true;
            Debug.Log("HA COLISIONAO CON LASTFLOOR");
        }
    }*/

}
