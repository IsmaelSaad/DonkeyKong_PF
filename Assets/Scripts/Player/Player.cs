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
    private bool enEscalera;
    public float speed;
    public float jumpSpeed;
    public float hDir;
    public float vDir;
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

    public Player(Transform transform, Transform raycastOrigin, float speed, float jumpSpeed, Rigidbody2D rb,  Animator animator, InputActionAsset inputActionMapping, LayerMask layer)
    {
        this.transform = transform;
        this.raycastOrigin = raycastOrigin;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
        this.layer = layer;
        this.enEscalera = false;
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
        Debug.Log(state);
        //Debug.DrawLine(raycastOrigin.position, raycastOrigin.position - raycastOrigin.up * 0.1f, Color.red);
    }

    public float GetPlayerSpeed() { 
        return speed * hDir;
    }

    public void EscaleraEnterTrigger(Collider2D collision)
    {
        if (collision.tag == "EscalerasEnter")
        {
            enEscalera = true;
            rb.gravityScale = 0;
        }
        else if (collision.tag == "EscalerasExit")
        {
            enEscalera = false;
            rb.gravityScale = 1;
        }
    }


    private void ChangeState()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -raycastOrigin.up, 0.1f, layer);

        vDir = ver_ia.ReadValue<float>(); 

        if ((enEscalera && vDir>0f) || (state == PLAYERSTATE.ONSTAIRIN))
        {
            state = PLAYERSTATE.ONSTAIRIN;
        }
        else
        {
            if (hit)
            {
                state = PLAYERSTATE.FLOOR;
            }
            else
            {
                state = PLAYERSTATE.JUMP;
            }
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
            case PLAYERSTATE.ONSTAIRIN:
                StairMovement();
                break;
        }
    }

    private void StairMovement()
    {
        float vertical = ver_ia.ReadValue<float>();

        if (Mathf.Abs(vertical) > 0.1f)
        {
            rb.velocity = new Vector2(0, vertical * speed);
        }
        else
        {
            rb.velocity = new Vector2(0, 0); 
        }
    }


    private void FloorMovement() { 
        hDir = hor_ia.ReadValue<float>();

        Run(hDir);

        if (jump_ia.triggered)
        {
            rb.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void JumpMovement() {
        hDir = hor_ia.ReadValue<float>();

        Run(hDir);
    }

    private void Run(float hDir) {
        rb.velocity = new Vector2 (speed* hDir, rb.velocity.y);

        if (hDir > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        if (hDir < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    

}