using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private float directionChangeTime = 3f;
    [SerializeField] LayerMask Firemask;
    [SerializeField] Vector2 movementPerSecond;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    
    void Update()
    {
        
    }
}
