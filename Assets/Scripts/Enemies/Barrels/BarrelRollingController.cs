using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador principal dels barrils rodons que delega funcionalitat a BarrelRolling
public class BarrelRollingController : MonoBehaviour
{
    [SerializeField] float speed; // Velocitat de moviment del barril
    [SerializeField] GameObject barrel; // Prefab del barril a instanciar
    [SerializeField] float bounceForce, groundRayDistance = 2.0f; // Par�metres de rebot i detecci�
    [SerializeField] LayerMask groundMaskRolling; // Capes considerades com a terra

    private Rigidbody2D rb; private CapsuleCollider2D crcColl; // Components f�sics
    private BarrelRolling barrelRoll; // Inst�ncia de la l�gica del barril

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crcColl = GetComponent<CapsuleCollider2D>();
        barrelRoll = new BarrelRolling(null, speed, gameObject, rb, crcColl,
                                    transform, barrel, bounceForce,
                                    groundRayDistance, groundMaskRolling);
    }

    void FixedUpdate() => barrelRoll.BarrelRollingFixedUpdate(); // Actualitzaci� f�sica

    void OnTriggerEnter2D(Collider2D collision) => barrelRoll.BarrelRollingOnTriggerEnter2D(collision); // Gesti� de col�lisions
}