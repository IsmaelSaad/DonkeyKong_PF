using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player 
{
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

    public Player(float speed, float jumpSpeed, Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping)
    {
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
    }

    public void WakePlayer() { 
        this.inputActionMapping.Enable();
        hor_ia = inputActionMapping.FindActionMap("H_Movement").FindAction("move");
        ver_ia = inputActionMapping.FindActionMap("V_Movement").FindAction("move");
        jump_ia = inputActionMapping.FindActionMap("Jumping").FindAction("jump");
    }

    public void StartPlayer()
    {
        this.state = PlayerState.IDLE;
    }

    public void UpdatePlayer() { 
        
    }

    

}