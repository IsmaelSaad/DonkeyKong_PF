using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Classe que controla el comportament del jugador
public class Player
{
    // Variables privades
    private Transform transform, raycastOrigin, actualFloor;
    private Rigidbody2D rb;
    private LayerMask layer;
    private bool enEscaleraUp, enEscaleraMid, enEscaleraDown, touchingDeath;
    private Vector3 actualEscalera;
    private SpriteRenderer hammerRenderer;
    public double numSwitches; // Nombre d'interruptors restants
    public float speed; // Velocitat de moviment
    public float jumpSpeed; // Força del salt
    public float hDir; // Direcció horitzontal (esquerra/dreta)
    public float vDir; // Direcció vertical (amunt/avall)
    public bool startPhase, onSwitch; // Estats del jugador
    public Animator animator, hammerAnimator; // Animadors
    public InputActionAsset inputActionMapping; // Controles d'entrada
    public Collider2D capsuleCollider; // Col·lider del jugador
    public BoxCollider2D hammerColl; // Col·lider del martell

    // Estats possibles del jugador
    public enum PLAYERSTATE
    {
        FLOOR,          // A terra
        AIR,            // A l'aire
        ONSTAIRSOUT,    // Sortint d'escala
        ONSTAIRSUP,     // Pujant escala
        ONSTAIRSDOWN,   // Baixant escala
        ONSTAIRSPHASE,  // Transició d'escala
        ONSTAIRSTOP,    // Aturat a escala
        DEATH,          // Mort
        HAMMERMODE,     // Mode martell
        FALLING         // Caient
    }
    public PLAYERSTATE state; // Estat actual

    // Accions d'entrada
    public InputAction hor_ia, ver_ia, jump_ia;

    // Constructor
    public Player(Transform transform, Transform raycastOrigin, float speed, float jumpSpeed,
                 Rigidbody2D rb, Animator animator, InputActionAsset inputActionMapping,
                 LayerMask layer, Collider2D capsuleCollider, Animator hammerAnimator,
                 SpriteRenderer hammerRenderer, BoxCollider2D hammerColl)
    {
        // Inicialitza totes les variables
        this.transform = transform;
        this.raycastOrigin = raycastOrigin;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.rb = rb;
        this.capsuleCollider = capsuleCollider;
        this.animator = animator;
        this.inputActionMapping = inputActionMapping;
        this.hammerAnimator = hammerAnimator;
        this.hammerRenderer = hammerRenderer;
        this.hammerColl = hammerColl;
        this.layer = layer;
        this.hammerColl = hammerColl;
    }

    // Activa els controls del jugador
    public void WakePlayer()
    {
        inputActionMapping.Enable();
        hor_ia = inputActionMapping.FindActionMap("H_Movement").FindAction("move");
        ver_ia = inputActionMapping.FindActionMap("V_Movement").FindAction("move");
        jump_ia = inputActionMapping.FindActionMap("Jumping").FindAction("jump");
    }

    // Inicialitza l'estat del jugador
    public void StartPlayer()
    {
        state = PLAYERSTATE.FLOOR;
        numSwitches = 8; // Inicialitza els interruptors
    }

    // Actualitza l'estat del jugador cada frame
    public void UpdatePlayer()
    {
        SwitchesEventCutscene(); // Comprova els interruptors

        // Control d'estats
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
            case PLAYERSTATE.ONSTAIRSPHASE:
                OnStairsPhase();
                break;
            case PLAYERSTATE.HAMMERMODE:
                OnHammerMode();
                break;
            case PLAYERSTATE.DEATH:
                OnDeath();
                break;
        }
    }

    // Corrutina que gestiona la fi del mode martell
    IEnumerator HammerModeEnds()
    {
        yield return new WaitForSeconds(7f); // Duració del mode martell

        // Efecte de parpelleig abans de desactivar el martell
        Color gold = new Color(1f, 1f, 0f);
        Color white = Color.white;
        float flashDuration = 0.2f;
        int flashCount = 3;

        for (int i = 0; i < flashCount; i++)
        {
            hammerRenderer.color = gold;
            yield return new WaitForSeconds(flashDuration);
            hammerRenderer.color = white;
            yield return new WaitForSeconds(flashDuration);
        }

        // Desactiva el martell
        hammerAnimator.enabled = false;
        hammerRenderer.enabled = false;
        hammerColl.enabled = false;

        // So de desactivació
        SoundController.Instance.sceneAudio.clip = SoundController.Instance.clip[5];
        SoundController.Instance.sceneAudio.Play();

        state = PLAYERSTATE.FLOOR; // Torna a l'estat normal
    }

    // Gestiona la mort del jugador
    private void OnDeath()
    {
        if (GameManager.Instance.GetLifes() != 0) // Si encara té vides
        {
            // Recarrega el nivell actual
            if (SceneManager.GetActiveScene().name == "Lvl1")
            {
                SceneManager.LoadScene("Lvl1");
            }
            else if (SceneManager.GetActiveScene().name == "Lvl2")
            {
                SceneManager.LoadScene("Lvl2");
            }
        }
    }

    // Mode martell
    private void OnHammerMode()
    {
        hDir = hor_ia.ReadValue<float>();
        Run(hDir); // Es pot moure normalment
    }

    // Quan està a terra
    private void OnFloor()
    {
        if (ToOnDeath()) return; // Comprova mort

        // Transicions a escales
        if (ver_ia.ReadValue<float>() > 0.5f && ToOnUpStairs() && !jump_ia.triggered) return;
        if (ToOnDownStairs()) return;
        if (ToOnAir()) return; // Si deixa de tocar terra

        // Moviment normal
        hDir = hor_ia.ReadValue<float>();
        Run(hDir);
        Jump(); // Pot saltar
    }

    // Quan està a l'aire
    private void OnAir()
    {
        if (ToOnDeath()) return;
        if (DetectFloor())
        {
            // Intentar anar a escales
            if (ver_ia.ReadValue<float>() > 0.5f && ToOnUpStairs() && DetectFloor()) return;
        }
        if (ToOnFloor()) return; // Si torna a terra

        // Moviment a l'aire
        hDir = hor_ia.ReadValue<float>();
        Run(hDir);
    }

    // Sortint d'escala
    private void OnStairsOut()
    {
        if (ToOnDeath()) return;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Quan acaba l'animació de sortida
        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            animator.SetBool("exitStair", false);
            state = PLAYERSTATE.ONSTAIRSPHASE; // Canvia a fase de transició
        }
    }

    // Fase de transició d'escala
    private void OnStairsPhase()
    {
        // Càlculs per posicionar correctament el jugador
        float angleDeg = actualFloor.rotation.eulerAngles.z;
        float angleRad = angleDeg * Mathf.Deg2Rad;
        float slopeLength = actualFloor.localScale.y;
        float verticalHeight = slopeLength * Mathf.Sin(angleRad);

        Debug.Log(verticalHeight);

        // Ajusta la posició
        if (angleDeg == 0)
        {
            transform.position += new Vector3(0, actualFloor.localScale.y + verticalHeight + 0.05f, 0);
        }
        else
        {
            transform.position += new Vector3(0, actualFloor.localScale.y + verticalHeight + 0.07f, 0);
        }

        actualEscalera = Vector3.zero;
        rb.gravityScale = 1f;
        state = PLAYERSTATE.ONSTAIRSDOWN; // Canvia a baixar escales
    }

    // Pujant escales
    private void OnUpStairs()
    {
        if (ToOnDeath()) return;
        vDir = ver_ia.ReadValue<float>();

        // Moviment vertical
        if (Mathf.Abs(vDir) > 0.1f)
        {
            rb.velocity = new Vector2(0, vDir * speed);
        }
        else
        {
            animator.StopPlayback();
            rb.velocity = Vector2.zero;
        }

        // Si surt de l'escala
        if (!enEscaleraUp && !enEscaleraMid)
        {
            animator.SetBool("idleStair", true);
            state = PLAYERSTATE.AIR;
            rb.gravityScale = 1;
        }

        // Si arriba al final de l'escala
        if (enEscaleraUp)
        {
            animator.SetBool("exitStair", true);
            rb.velocity = Vector2.zero;
            state = PLAYERSTATE.ONSTAIRSOUT;
        }
    }

    // Baixant escales
    private void OnDownStairs()
    {
        if (ToOnDeath()) return;
        hDir = hor_ia.ReadValue<float>();

        if (ToOnUpStairs()) return;

        // Moviment horitzontal
        if (hDir != 0)
        {
            animator.SetBool("idleStair", false);
            if (ToOnFloor()) return; // Si torna a terra
        }
    }

    // Detecta si està tocant terra
    private bool DetectFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, -Vector2.up, 0.1f, layer);
        Debug.DrawRay(raycastOrigin.position, -Vector2.up * 0.1f, Color.green);
        return hit.collider != null;
    }

    // Funció de salt
    private void Jump()
    {
        if (jump_ia.triggered && state == PLAYERSTATE.FLOOR)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    // Funció de córrer
    private void Run(float hDir)
    {
        rb.velocity = new Vector2(speed * hDir, rb.velocity.y);

        // Gira el personatge segons la direcció
        if (hDir > 0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (hDir < -0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }

    // Comprova si s'han activat tots els interruptors
    public void SwitchesEventCutscene()
    {
        if (numSwitches == 0)
        {
            SceneManager.LoadScene("Cutscene3"); // Carrega escena final
        }
    }

    // Gestiona col·lisió amb interruptor
    public void SwitchCollisionEnter(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            onSwitch = true;
        }
    }

    // Gestiona sortida de col·lisió amb interruptor
    public void SwitchCollisionExit(Collider2D collision)
    {
        if (collision.CompareTag("Switch") && onSwitch)
        {
            numSwitches--; // Decrementa comptador
            onSwitch = false;
        }
    }

    // Gestiona col·lisió amb esdeveniment
    public void EventCollisionEnter(Collider2D collision)
    {
        if (state == PLAYERSTATE.ONSTAIRSDOWN && collision.CompareTag("Event"))
        {
            SceneManager.LoadScene("Cutscene2"); // Carrega escena
        }
    }

    // Gestiona col·lisió amb martell
    public void HammerCollisionEnter(Collider2D collision)
    {
        if (collision.CompareTag("Hammer"))
        {
            state = PLAYERSTATE.HAMMERMODE; // Activa mode martell
            SoundController.Instance.sceneAudio.clip = SoundController.Instance.clip[7];
            SoundController.Instance.sceneAudio.Play();
            hammerAnimator.enabled = true;
            hammerRenderer.enabled = true;
            hammerColl.enabled = true;
            CoroutineRunner.Instance.StartCoroutine(HammerModeEnds()); // Inicia temporitzador
        }
    }

    // Gestiona col·lisió amb sòl que canvia direcció
    public void SueloCollisionEnter(Collision2D collision)
    {
        if (collision.collider.CompareTag("ChangeDirection"))
        {
            actualFloor = collision.collider.transform;
        }
    }

    // Gestiona col·lisió amb barril (trigger)
    public void BarrilTriggerEnter(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if ((collision.CompareTag("BarrelRolling") || collision.CompareTag("BarrelDeath") || collision.CompareTag("Flame")) && state != PLAYERSTATE.HAMMERMODE)
        {
            GameManager.Instance.DecrementLife(1); // Perd vida
            touchingDeath = true;
        }
    }

    // Gestiona col·lisió amb barril
    public void BarrilCollisionEnter(Collision2D collision)
    {
        if ((collision.collider.CompareTag("Barrel") || collision.collider.CompareTag("BarrelRolling") || collision.collider.CompareTag("Flame")) && state != PLAYERSTATE.HAMMERMODE)
        {
            GameManager.Instance.DecrementLife(1); // Perd vida
            touchingDeath = true;
        }
    }

    // Gestiona entrada a zona d'escala
    public void EscalerasCollisionEnter(Collider2D collision)
    {
        if (collision.CompareTag("Escaleras"))
        {
            actualEscalera = collision.transform.position;
        }

        // Marca les diferents parts de l'escala
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

    // Gestiona sortida de zona d'escala
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

    // Transició a estat "a terra"
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

    // Transició a estat "a l'aire"
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

    // Transició a estat "mort"
    bool ToOnDeath()
    {
        if (touchingDeath)
        {
            state = PLAYERSTATE.DEATH;
            return true;
        }
        return false;
    }

    // Transició a estat "pujant escales"
    bool ToOnUpStairs()
    {
        if (enEscaleraDown && ver_ia.ReadValue<float>() > 0.5f && !jump_ia.triggered && actualEscalera != Vector3.zero)
        {
            transform.position = new Vector2(actualEscalera.x, transform.position.y);
            state = PLAYERSTATE.ONSTAIRSUP;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            return true;
        }
        return false;
    }

    // Transició a estat "baixant escales"
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