using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player 
{
    private Transform transform;
    private float speed;
    private float jumpSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public InputActionAsset inputActionMapping;

    InputAction hor_ia, ver_ia, jump_ia;

    enum PlayerState {
        IDLE, RUN,
        JUMP, ONSTAIROUT,
        ONSTAIRIN, ONSTAIRSUP,
        ONSTAIRSDOWN, DEATH,
        HAMMERIDLE, HAMMERWALK,
        FALLING
    }
    private PlayerState state;

    public Player(Transform transform, float speed, float jumpSpeed, Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping)
    {
        this.transform = transform;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
    }

    public void WakePlayer() { 
        inputActionMapping.Enable();
        hor_ia = inputActionMapping.FindActionMap("H_Movement").FindAction("move");
        ver_ia = inputActionMapping.FindActionMap("V_Movement").FindAction("move");
        jump_ia = inputActionMapping.FindActionMap("Jumping").FindAction("jump");
    }

    public void StartPlayer()
    {
        this.state = PlayerState.IDLE;
    }

    public void UpdatePlayer() {
        FloorMovement();
    }

    private void UpdateState() { 
        
    }

    private void FloorMovement() {
        float mx = hor_ia.ReadValue<float>();
        float my = ver_ia.ReadValue<float>();
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