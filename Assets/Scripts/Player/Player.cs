using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player
{
    private Transform transform;
    private Transform raycastOrigin;
    private Rigidbody2D rb;
    private LayerMask layer;
    private bool enEscaleraUp, enEscaleraMid, enEscaleraDown;
    private Vector3 actualEscalera;
    public float speed;
    public float jumpSpeed;
    public float hDir;
    public float vDir;
    public Animator animator;
    public InputActionAsset inputActionMapping;
    public Collider2D capsuleCollider;

    public enum PLAYERSTATE
    {
        FLOOR, AIR,
        ONSTAIRSOUT,
        ONSTAIRSUP, ONSTAIRSDOWN,
        DEATH, HAMMERIDLE,
        HAMMERWALK, FALLING
    }
    public PLAYERSTATE state;

    public InputAction hor_ia, ver_ia, jump_ia;

    public Player(Transform transform, Transform raycastOrigin, float speed, float jumpSpeed, Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping, LayerMask layer, Collider2D capsuleCollider)
    {
        this.transform = transform;
        this.raycastOrigin = raycastOrigin;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.capsuleCollider = capsuleCollider;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
        this.layer = layer;
    }

    public void WakePlayer()
    {
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
        Debug.Log(state);

        switch (state)
        {
            case PLAYERSTATE.FLOOR:
                OnFloor();
                break;
            case PLAYERSTATE.AIR:
                OnAir();
                break;
            case PLAYERSTATE.ONSTAIRSOUT:
                OnStairsOut();
                break;
            case PLAYERSTATE.ONSTAIRSUP:
                OnUpStairs();
                break;
            case PLAYERSTATE.ONSTAIRSDOWN:
                OnDownStairs();
                break;
        }
    }

    private void OnFloor()
    {
        // Primero verificar transición a escaleras
        if (ver_ia.ReadValue<float>() > 0.5f && ToOnUpStairs() && !jump_ia.triggered) return;

        if (ToOnDownStairs()) return;
        if (ToOnAir()) return;

        // Movimiento normal
        hDir = hor_ia.ReadValue<float>();
        Run(hDir);
        Jump();
    }

    private void OnAir()
    {
        if (DetectFloor()) {
            // Intentar transición a escaleras primero
            if (ver_ia.ReadValue<float>() > 0.5f && ToOnUpStairs() && DetectFloor()) return;
        }

        if (ToOnFloor()) return;

        hDir = hor_ia.ReadValue<float>();
        Run(hDir);
    }

    // sin uso
    /*
    public void IgnoreOutStairs(Collision2D collision) {
        if (exitingStair)
        {
            if ((collision.gameObject.layer == 6) && rb.velocity.y > 0)
            {
                Physics2D.IgnoreCollision(capsuleCollider, collision.collider, exitingStair);
            }
        }
        else {
            if (collision.gameObject.layer == 6)
            {
                Physics2D.IgnoreCollision(capsuleCollider, collision.collider, exitingStair);
            }
        }       
    }*/

    private void OnStairsOut()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0)) {
            transform.position += new Vector3(0,0.35f,0);
            rb.gravityScale = 1.0f;
            Debug.Log("finishexit");
            animator.SetBool("exitStair", false);
            state = PLAYERSTATE.ONSTAIRSDOWN;
        }
    }

    private void OnUpStairs()
    {
        vDir = ver_ia.ReadValue<float>();

        if (Mathf.Abs(vDir) > 0.1f)
        {
            rb.velocity = new Vector2(0, vDir * speed);
        }
        else
        {
            animator.StopPlayback();
            rb.velocity = Vector2.zero;
        }

        // Salir si ya no estamos en escalera
        if (!enEscaleraUp && !enEscaleraMid)
        {
            animator.SetBool("idleStair", true);
            state = PLAYERSTATE.AIR;
            rb.gravityScale = 1;
        }

        if (enEscaleraUp) {
            animator.SetBool("exitStair", true);
            rb.velocity = Vector2.zero;
            state = PLAYERSTATE.ONSTAIRSOUT;
        }
    }

    private void OnDownStairs()
    {
        hDir = hor_ia.ReadValue<float>();

        if (ToOnUpStairs()) return;

        if (hDir != 0) {
            animator.SetBool("idleStair", false);
            if (ToOnFloor()) return;
        }
    }

    private bool DetectFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -Vector2.up, 0.1f, layer);
        Debug.DrawRay(raycastOrigin.position, -Vector2.up * 0.1f, Color.green);
        return hit.collider != null;
    }

    private void Jump()
    {
        if (jump_ia.triggered && state == PLAYERSTATE.FLOOR)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void Run(float hDir)
    {
        rb.velocity = new Vector2(speed * hDir, rb.velocity.y);

        if (hDir > 0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (hDir < -0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    public void EscalerasCollisionEnter(Collider2D collision)
    {
        if (collision.CompareTag("Escaleras"))
        {
            actualEscalera = collision.transform.position;
        }

        if (collision.CompareTag("EscalerasEnter"))
        {
            enEscaleraDown = true;
        }
        else if (collision.CompareTag("EscalerasMiddle"))
        {
            enEscaleraMid = true;
        }
        else if (collision.CompareTag("EscalerasExit"))
        {
            enEscaleraUp = true;
        }
    }

    public void EscalerasCollisionExit(Collider2D collision)
    {
        if (collision.CompareTag("EscalerasEnter"))
        {
            enEscaleraDown = false;
        }
        else if (collision.CompareTag("EscalerasMiddle"))
        {
            enEscaleraMid = false;
        }
        else if (collision.CompareTag("EscalerasExit"))
        {
            enEscaleraUp = false;
        }
    }

    bool ToOnFloor()
    {
        if (DetectFloor() && rb.velocity.y <= 0)
        {
            state = PLAYERSTATE.FLOOR;
            rb.gravityScale = 1;
            return true;
        }
        return false;
    }

    bool ToOnAir()
    {
        if (!DetectFloor())
        {
            state = PLAYERSTATE.AIR;
            rb.gravityScale = 1;
            return true;
        }
        return false;
    }

    /*
    bool ToOnStairs()
    {
        if (enEscaleraMid)
        {
            state = PLAYERSTATE.ONSTAIRS;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            return true;
        }
        return false;
    }*/

    bool ToOnUpStairs()
    {

        if (enEscaleraDown  && ver_ia.ReadValue<float>() > 0.5f && !jump_ia.triggered)
        {
            transform.position = new Vector2(actualEscalera.x, transform.position.y);
            state = PLAYERSTATE.ONSTAIRSUP;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            return true;
        }
        return false;
    }

    bool ToOnDownStairs()
    {
        if (animator.GetBool("idleStair"))
        {
            state = PLAYERSTATE.ONSTAIRSDOWN;
            rb.gravityScale = 1;
            return true;
        }
        return false;
    }
}