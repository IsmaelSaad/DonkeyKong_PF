using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrelRolling : Enemy
{
    private GameObject itself;
    private Rigidbody2D rb;
    private CapsuleCollider2D crcColl;
    private Transform transform;
    private GameObject barrel;
    private float bounceForce;
    private float groundRayDistance = 2.0f;
    private LayerMask groundMaskRolling;
    private bool hasGround;
    private int barrelRollPoints = 300;

    enum State
    {
        MOVEMENT, FALLING,
        BOUNCING, BOUNCING_FALL,
        DESTROY
    };
    State state = State.MOVEMENT;

    public BarrelRolling(Animator animator, float speed, GameObject itself, Rigidbody2D rb, CapsuleCollider2D crcColl, Transform transform, GameObject barrel, float bounceForce, float groundRayDistance, LayerMask groundMaskRolling) : base(animator, speed) {
        this.rb = rb;
        this.itself = itself;
        this.crcColl = crcColl;
        this.transform = transform;
        this.barrel = barrel;
        this.bounceForce = bounceForce;
        this.groundRayDistance = groundRayDistance;
        this.groundMaskRolling = groundMaskRolling;
    }

    public void BarrelRollingFixedUpdate()
    {
        Debug.Log(state);

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
                if (hit2D.collider != null) {
                    if (hit2D.collider.CompareTag("LastFloor") || hit2D.collider.CompareTag("OilBarrel"))
                    {
                        GameObject barrelInstantiate = Object.Instantiate(barrel, transform.position, Quaternion.identity);
                        if (barrelInstantiate != null)
                            barrelInstantiate.GetComponent<BarrelController>().speed *= -1;
                        Object.Destroy(itself);
                        crcColl.enabled = true;
                    }
                }
                break;
            case State.BOUNCING:
                speed = Random.Range(-5f, 5f);
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
            case State.DESTROY:
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                break;
        }
    }

    public void BarrelRollingOnTriggerEnter2D(Collider2D collision) 
    {   
        if (collision.CompareTag("OilBarrel"))
        {
            Object.Destroy(itself);
        }
    }
}
