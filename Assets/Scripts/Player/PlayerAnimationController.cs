using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
using UnityEngine;

// Controlador d'animacions del jugador
public class PlayerAnimationController : MonoBehaviour
{
    PlayerController pc;    // Controlador del jugador
    Player mario;           // Instància del jugador (Mario)

    // Inicialització abans del primer frame
    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();  // Busca el controlador
        mario = pc.GetMario();                      // Obtenir la instància de Mario
    }

    void Start()
    {
        // Inicialització addicional si és necessària
    }

    // Actualització cada frame
    void Update()
    {
        // Si Mario no existeix, intenta trobar-lo de nou
        if (mario == null)
        {
            pc = FindObjectOfType<PlayerController>();
            if (pc != null)
            {
                mario = pc.GetMario();
            }
            return;
        }

        // Control d'animacions segons l'estat del jugador
        switch (mario.state)
        {
            case (Player.PLAYERSTATE.FLOOR):    // A terra
                mario.animator.SetBool("hammerMode", false);
                mario.animator.SetBool("onStair", false);
                mario.animator.SetBool("jumping", false);
                mario.hDir = mario.hor_ia.ReadValue<float>();
                mario.animator.SetFloat("speed", Mathf.Abs(mario.speed * mario.hDir));
                break;

            case (Player.PLAYERSTATE.AIR):      // A l'aire
                mario.animator.SetBool("jumping", true);
                break;

            case (Player.PLAYERSTATE.HAMMERMODE): // Mode martell
                mario.animator.SetBool("hammerMode", true);
                mario.hDir = mario.hor_ia.ReadValue<float>();
                mario.animator.SetFloat("speed", Mathf.Abs(mario.speed * mario.hDir));
                break;

            case (Player.PLAYERSTATE.ONSTAIRSUP): // Pujant escales
                mario.animator.SetBool("jumping", false);
                mario.animator.SetBool("onStair", true);
                mario.animator.SetFloat("speed", 0);
                mario.animator.SetFloat("speedStair", Mathf.Abs(mario.speed * mario.vDir));
                break;

            case (Player.PLAYERSTATE.ONSTAIRSDOWN): // Baixant escales
                mario.animator.SetBool("onStair", false);
                mario.animator.SetBool("idleStair", true);
                break;

                /* (Exemple de codi comentat per possibles ampliacions)
                case (Player.PLAYERSTATE.HAMMERIDLE):
                    mario.animator.SetBool("")
                */
        }
    }
}