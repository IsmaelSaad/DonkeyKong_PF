using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    PlayerController pc;
    Player mario;
    // Start is called before the first frame update
    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        mario = pc.GetMario();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // encontrar a mario si no existe aún 
        if (mario == null)
        {
            pc = FindObjectOfType<PlayerController>();
            if (pc != null)
            {
                mario = pc.GetMario();
            }
            return;
        }

        switch (mario.state)
        {
            case (Player.PLAYERSTATE.FLOOR):
                mario.animator.SetBool("jumping", false);
                mario.animator.SetFloat("speed", Mathf.Abs(mario.speed * mario.hDir));
                break;
            case (Player.PLAYERSTATE.AIR):
                mario.animator.SetBool("jumping", true);
                break;
            case (Player.PLAYERSTATE.ONSTAIRSUP):
                mario.animator.SetBool("onStair", true);
                mario.animator.SetFloat("speedStair", Mathf.Abs(mario.speed * mario.vDir));
                break;
            case (Player.PLAYERSTATE.ONSTAIRSDOWN):
                mario.animator.SetBool("jumping", false);
                mario.animator.SetBool("onStair", false);
                mario.animator.SetBool("idleStair", true);
                break;
        }
    }
}
