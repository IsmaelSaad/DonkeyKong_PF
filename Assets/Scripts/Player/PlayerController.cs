using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player;


public class PlayerController : MonoBehaviour
{

    [SerializeField]
    LayerMask floorLayer;
    [SerializeField]
    Transform raycastOrigin;
    [SerializeField]
    public InputActionAsset inputActionMapping;
    [SerializeField]
    float speed, jumpSpeed;
    [SerializeField]
    Animator animator;

    Rigidbody2D rb;
    

    public Player mario;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mario = new Player(transform, raycastOrigin, speed, jumpSpeed, rb, animator, inputActionMapping, floorLayer);
        mario.WakePlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        mario.StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        mario.UpdatePlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mario.EscaleraEnterTrigger(collision);
    }


    public Player GetMario() { 
        return mario;
    }
}
