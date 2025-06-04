using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Classe que gestiona els sons del jugador
public class PlayerSoundManager : MonoBehaviour
{
    private AudioSource audioSource; // Font d'àudio

    [SerializeField] AudioClip[] sounds; // Array de sons disponibles
    [SerializeField] InputActionAsset ia_asset; // Accions d'entrada
    public AudioClip actualSound; // So actual que s'està reproduint

    InputAction ia_jump, ia_hor; // Accions de salt i moviment horitzontal

    // Inicialització
    private void Start()
    {
        ia_asset.Enable(); // Activa els controls
        ia_jump = ia_asset.FindActionMap("Jumping").FindAction("jump"); // Configura acció de salt
        ia_hor = ia_asset.FindActionMap("H_Movement").FindAction("move"); // Configura acció de moviment

        audioSource = GetComponent<AudioSource>(); // Obtenir component AudioSource
    }

    // Actualització cada frame
    private void Update()
    {
        // Si el jugador salta
        if (ia_jump.triggered)
        {
            actualSound = sounds[0]; // Selecciona el so de salt (primer element de l'array)
            audioSource.clip = actualSound; // Assigna el so
            audioSource.Play(); // Reprodueix el so
        }

        // Si el jugador es mou horitzontalment
        if (ia_hor.ReadValue<float>() != 0f)
        {
            audioSource.Play(); // Reprodueix el so de moviment
        }
    }
}