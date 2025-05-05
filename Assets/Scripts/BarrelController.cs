using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BarrelController : MonoBehaviour
{
    

    [SerializeField] float speed;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask;
    

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;

    CircleCollider2D crcColl;

    bool hasGround = false;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL };
    State state = State.MOVEMENT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            case State.FALLING:
                rb.velocity = new Vector2(speed, rb.velocity.y);
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

    


}
 