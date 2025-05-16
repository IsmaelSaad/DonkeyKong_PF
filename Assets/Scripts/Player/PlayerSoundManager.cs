using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioClip[] sounds;
    [SerializeField] InputActionAsset ia_asset;
    public AudioClip actualSound;

    InputAction ia_jump, ia_hor;
    
    private void Start()
    {
        ia_asset.Enable();
        ia_jump = ia_asset.FindActionMap("Jumping").FindAction("jump");
        ia_hor = ia_asset.FindActionMap("H_Movement").FindAction("move");

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {       
        if (ia_jump.triggered)
        {
            actualSound = sounds[0];
            audioSource.clip = actualSound;
            audioSource.Play();

        }
        if (ia_hor.ReadValue<float>() != 0f)
        {
            audioSource.Play();
        }   
    }
}

