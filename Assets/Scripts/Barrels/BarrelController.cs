using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class BarrelController : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] public float speed;
    [SerializeField] Transform raycastOriginStairs;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask, stairMask, playerMask;
    
    GameManager gameManager;

    int downStairs;

    //RigiBody para la clase de Enemies
    Rigidbody2D rb;
    string playerPoints;
    

    [SerializeField] BoxCollider2D playerColl, boxColl;
    [SerializeField] CircleCollider2D crcColl;

    bool hasGround = false, hasStairs = false, hasPlayerOn = false, isPlayerTouchingPoints = false;
    int barrelPoints = 100;

    enum State { MOVEMENT, FALLING, BOUNCING, BOUNCING_FALL , ONSTAIRS, FALLSTAIRS};
    State state = State.MOVEMENT;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.FALLSTAIRS && collision.CompareTag("EscalerasExit")) {
            Debug.Log("asda");
        }

        

        if (playerColl.IsTouching(collision) && !isPlayerTouchingPoints) 
        {
            if (collision.CompareTag("Player"))
            {
                hasPlayerOn = true;
                isPlayerTouchingPoints = true;
            }
        } 

        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerColl.IsTouching(collision))
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerTouchingPoints = true;
            }
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerColl.IsTouching(collision))
        {
            if (collision.CompareTag("Player"))
            {
                hasPlayerOn = false;
                isPlayerTouchingPoints = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (hasPlayerOn && isPlayerTouchingPoints)
        {
            hasPlayerOn = false;
            
            gameManager.AddPoints(barrelPoints);
            //playerPoints = barrelPoints;

            playerPoints = GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = gameManager.GetPoints().ToString();
        }
        Debug.Log(gameManager.GetPoints());
        

        

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
                
                
                break;
            case State.FALLSTAIRS:
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("falling", true);
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
 