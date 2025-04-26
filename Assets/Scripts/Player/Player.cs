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
    public float speed;
    public float jumpSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public InputActionAsset inputActionMapping;
    private LayerMask layer;

    InputAction hor_ia, ver_ia, jump_ia;

    enum PLAYERSTATE {
        FLOOR,
        JUMP, ONSTAIROUT,
        ONSTAIRIN, ONSTAIRSUP,
        ONSTAIRSDOWN, DEATH,
        HAMMERIDLE, HAMMERWALK,
        FALLING
    }
    private PLAYERSTATE state;

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
        Debug.DrawLine(raycastOrigin.position, raycastOrigin.position - raycastOrigin.up * 0.3f, Color.red);
    }


    private void ChangeState() {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -raycastOrigin.up, 0.3f, layer);

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
        }
    }

    private void FloorMovement() { 
        float mx = hor_ia.ReadValue<float>();

        Run(mx);
    }

    private void Run(float mx) {
        rb.velocity = new Vector2 (speed*mx, rb.velocity.y);

        if (mx > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        if (mx < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        if (jump_ia.triggered) {
            rb.AddForce(new Vector2(0,1) * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    

}