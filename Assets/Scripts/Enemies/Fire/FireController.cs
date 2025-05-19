using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 3f;

    Fire fire;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fire = new Fire(animator, moveSpeed, directionChangeInterval, rb);
        fire.EnemyStart();
    }

    
    void Update()
    {
        fire.EnemyUpdate();
    }
}
