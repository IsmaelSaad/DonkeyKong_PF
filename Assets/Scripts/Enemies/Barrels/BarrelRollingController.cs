using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRollingController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject barrel;
    [SerializeField] float bounceForce;
    [SerializeField] float groundRayDistance = 2.0f;
    [SerializeField] LayerMask groundMaskRolling;
    [SerializeField] CapsuleCollider2D crcCollDeath;

    Rigidbody2D rb;
    CapsuleCollider2D crcColl;

    BarrelRolling barrelRoll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CapsuleCollider2D>();
        barrelRoll = new BarrelRolling(null, speed, gameObject, rb, crcColl, transform, barrel, bounceForce, groundRayDistance, groundMaskRolling, crcCollDeath);
    }

    void FixedUpdate()
    {
        barrelRoll.BarrelRollingFixedUpdate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        barrelRoll.BarrelRollingOnTriggerEnter2D(collision);
    }
}
