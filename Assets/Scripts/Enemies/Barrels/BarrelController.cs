using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public float speed;
    [SerializeField] BoxCollider2D detectStair, playerPointsColl;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f, stairRayDistance = 2.0f;
    [SerializeField] LayerMask groundMask;

    GameObject itself;
    GameManager gameManager;

    Rigidbody2D rb;
    BoxCollider2D boxColl;

    Barrel barrel;

    // Start is called before the first frame update
    void Start()
    {
        itself = gameObject;
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponent<Animator>();
        barrel = new Barrel(transform, animator, speed, rb, boxColl, detectStair, playerPointsColl, bounceForce, groundRayDistance, stairRayDistance, groundMask, gameManager, itself);
    }

    void FixedUpdate()
    {
        barrel.BarrelFixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        barrel.PointsOnTriggerEnter2D(collision);
        barrel.BarrelOnTriggerEnter2D(collision);
        

        if (collision.CompareTag("OilBarrel"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        barrel.BarrelOnTriggerExit2D(collision);
        barrel.PointsOnTriggerExit2D(collision);
    }
}
