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
    private bool enEscalera, escalandoEscalera;
    public float speed;
    public float jumpSpeed;
    public float hDir;
    public float vDir;
    public Animator animator;
    public InputActionAsset inputActionMapping;

    public enum PLAYERSTATE {
        FLOOR, AIR, 
        ONSTAIRS, 
        ONSTAIRSUP, ONSTAIRSDOWN, 
        DEATH, HAMMERIDLE, 
        HAMMERWALK, FALLING
    }
    public PLAYERSTATE state;

    public InputAction hor_ia, ver_ia, jump_ia;

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

    public float GetPlayerSpeed()
    {
        return speed * hDir;
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
        State();
        //Debug.Log(enEscalera);
        //Debug.DrawLine(raycastOrigin.position, raycastOrigin.position - raycastOrigin.up * 0.1f, Color.red);
    }

    private void State() {
        switch (state)
        { 
            case PLAYERSTATE.FLOOR:
                OnFloor();
                break;
            case PLAYERSTATE.AIR:
                OnAir();
                break;
            case PLAYERSTATE.ONSTAIRS:
                OnStairs();
                break;
            case PLAYERSTATE.ONSTAIRSUP:
                OnTopStairs();
                break;
            case PLAYERSTATE.ONSTAIRSDOWN:
                OnBottomStairs();
                break;
        }
    }

    private void OnFloor()
    {
        if (ToOnAir())
        {
            return;
        }

        if (ToOnDownStairs())
        {
            return;
        }

        if (ToOnUpStairs())
        {
            return;
        }

        hDir = hor_ia.ReadValue<float>();

        Run(hDir);

        Jump();
    }

    private void OnAir() 
    {
        if (ToOnFloor()) {
            return;
        }

        hDir = hor_ia.ReadValue<float>();
        Run(hDir);
    }

    private void OnStairs()
    {
        if (ToOnUpStairs()) {
            return;
        }

        if (ToOnDownStairs()) {
            return;
        }
    }

    private void OnTopStairs() 
    {
        if (ToOnFloor())
        {
            return;
        }

        if (ToOnStairs())
        {
            return;
        }

        if (ToOnAir())
        {
            return;
        }
    }

    private void OnBottomStairs()
    {
        if (ToOnStairs())
        {
            return;
        }

        if (ToOnFloor())
        {
            return;
        }

        if (ToOnAir())
        {
            return;
        }



    }

    private bool DetectFloor() {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -raycastOrigin.up, 0.1f, layer);
        return hit ? true : false;
    }


    private void Jump() {
        if (jump_ia.triggered)
        {
            rb.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
        }
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

    bool ToOnFloor() 
    {
        return false;
    }

    bool ToOnAir()
    {
        return false;
    }

    bool ToOnStairs()
    {
        return false;
    }

    bool ToOnDownStairs()
    {
        return false;
    }

    bool ToOnUpStairs()
    {
        return false;
    }

    /*
    public void EscaleraEnterTrigger(Collider2D collision)
    {
        //vDir = ver_ia.ReadValue<float>();
        if ((collision.tag == "EscalerasEnter"))
        {
            enEscalera = true;
            rb.gravityScale = 0;
        }
    }

    public void EscaleraExitTrigger(Collider2D collision)
    {
        //vDir = ver_ia.ReadValue<float>();
        if ((collision.tag == "EscalerasExit"))
        {
            escalandoEscalera = false;
            rb.gravityScale = 1;
        }
    }
    */


}