using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player 
{
    private Transform transform;
    private Transform raycastOrigin;
    private Rigidbody2D rb;
    private LayerMask layer;
    public float speed;
    public float dir;
    public float jumpSpeed;
    public Animator animator;
    public InputActionAsset inputActionMapping;
    

    public enum PLAYERSTATE {
        FLOOR,
        JUMP, ONSTAIROUT,
        ONSTAIRIN, ONSTAIRSUP,
        ONSTAIRSDOWN, DEATH,
        HAMMERIDLE, HAMMERWALK,
        FALLING
    }
    public PLAYERSTATE state;

    InputAction hor_ia, ver_ia, jump_ia;

    public Player(Transform transform, Transform raycastOrigin, float speed, float jumpSpeed, Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping, LayerMask layer)
    {
        this.transform = transform;
        this.raycastOrigin = raycastOrigin;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
        this.layer = layer;
    }

    public void WakePlayer() { 
        inputActionMapping.Enable();
        hor_ia = inputActionMapping.FindActionMap("H_Movement").FindAction("move");
        ver_ia = inputActionMapping.FindActionMap("V_Movement").FindAction("move");
        jump_ia = inputActionMapping.FindActionMap("Jumping").FindAction("jump");
    }

    public void StartPlayer()
    {
        state = PLAYERSTATE.FLOOR;
    }

    public void UpdatePlayer() 
    {
        ChangeState();
        State();
        Debug.DrawLine(raycastOrigin.position, raycastOrigin.position - raycastOrigin.up * 0.1f, Color.red);
    }

    public float GetPlayerSpeed() { 
        return speed * dir;
    }


    private void ChangeState() {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -raycastOrigin.up, 0.1f, layer);

        if (hit)
        {
            state = PLAYERSTATE.FLOOR;
        }
        else 
        {
            state = PLAYERSTATE.JUMP;
        }
    }

    private void State() {
        switch (state)
        { 
            case PLAYERSTATE.FLOOR:
                FloorMovement();
                break;
            case PLAYERSTATE.JUMP:
                JumpMovement();
                break;
        }
    }

    private void FloorMovement() { 
        dir = hor_ia.ReadValue<float>();

        Run(dir);

        if (jump_ia.triggered)
        {
            rb.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void JumpMovement() {
        dir = hor_ia.ReadValue<float>();

        Run(dir);
    }

    private void Run(float dir) {
        rb.velocity = new Vector2 (speed* dir, rb.velocity.y);

        if (dir > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        if (dir < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    

}