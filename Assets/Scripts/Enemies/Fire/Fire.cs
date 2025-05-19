using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : Enemy
{
    private float latestDirectionChangeTime;
    private float directionChangeInterval;
    private float moveSpeed;
    private int currentDirection = 1; // 1 for right, -1 for left
    private Rigidbody2D rb;

    // Constructor that initializes the Fire enemy
    public Fire(Animator animator, float speed, float directionChangeInterval, Rigidbody2D rb) : base(animator, speed)
    {
        this.rb = rb;
        this.directionChangeInterval = directionChangeInterval;
        this.moveSpeed = speed;
    }


    public override void EnemyStart()
    {
        latestDirectionChangeTime = Time.time;
    }


    public override void EnemyUpdate()
    {
        // Change direction periodically
        if (Time.time - latestDirectionChangeTime > directionChangeInterval)
        {
            currentDirection *= -1;
            latestDirectionChangeTime = Time.time;
        }

        // Move the fire enemy
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(currentDirection * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Change direction when hitting a wall or obstacle
    //    if (collision.gameObject.CompareTag("ChangeDirection"))
    //    {
    //        currentDirection *= -1;
    //    }
    //}


}