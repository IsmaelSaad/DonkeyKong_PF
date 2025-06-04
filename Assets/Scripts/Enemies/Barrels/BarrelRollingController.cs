using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador principal dels barrils rodons que delega funcionalitat a BarrelRolling
public class BarrelRollingController : MonoBehaviour
{
    [SerializeField] float speed; // Velocitat de moviment del barril
    [SerializeField] GameObject barrel; // Prefab del barril a instanciar
    [SerializeField] float bounceForce, groundRayDistance = 2.0f; // Paràmetres de rebot i detecció
    [SerializeField] LayerMask groundMaskRolling; // Capes considerades com a terra

    private Rigidbody2D rb; private CapsuleCollider2D crcColl; // Components físics
    private BarrelRolling barrelRoll; // Instància de la lògica del barril

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CapsuleCollider2D>();
        barrelRoll = new BarrelRolling(null, speed, gameObject, rb, crcColl,
                                    transform, barrel, bounceForce,
                                    groundRayDistance, groundMaskRolling);
    }

    void FixedUpdate() => barrelRoll.BarrelRollingFixedUpdate(); // Actualització física

    void OnTriggerEnter2D(Collider2D collision) => barrelRoll.BarrelRollingOnTriggerEnter2D(collision); // Gestió de col·lisions
}