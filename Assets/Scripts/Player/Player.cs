using UnityEngine;
using UnityEngine.InputSystem;

public class Player 
{
    public float speed;
    public float jumpSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    private InputActionAsset inputActionMapping;

    InputAction hor_ia, ver_ia, jump_ia;

    enum PlayerState {
        IDLE, RUN,
        JUMP, ONSTAIROUT,
        ONSTAIRIN, ONSTAIRSUP,
        ONSTAIRSDOWN, DEATH,
        HAMMERIDLE, HAMMERWALK,
        FALLING
    }
    PlayerState state;

    public Player(float speed, float jumpSpeed, Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping)
    {
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.animator = animator;
    }

    public void WakePlayer() { 
        this.inputActionMapping.Enable();

    }

    public void StartPlayer()
    {

    }

    public void UpdatePlayer() { 
        
    }

    

}