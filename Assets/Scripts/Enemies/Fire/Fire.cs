using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : Enemy
{
    private float LatestDirectionMoved;
    private float directionChangeTime = 3f;
    private float characterVelocity = 2f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    public LayerMask Firemask;
    Rigidbody2D rb;

    public Fire (Animator animator, float speed, float LatestDirectionMoved, float directionChangeTime, float characterVelocity, LayerMask Firemask, Vector2 movementDirection, Vector2 movementPerSecond, Rigidbody2D rb) : base (animator, speed) 
    {
        this.animator = animator;
        this.LatestDirectionMoved = LatestDirectionMoved;
        this.directionChangeTime = directionChangeTime;
        this.characterVelocity = characterVelocity;
        this.movementDirection = movementDirection;
        this.movementPerSecond = movementPerSecond;
        this.Firemask = Firemask;    
    }

    void Start()
    {
        LatestDirectionMoved = 0f;
        
    }

    void Update() 
    {
    }


}