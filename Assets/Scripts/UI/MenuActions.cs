using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

// Controlador de las acciones y navegación en los menús del juego
public class MenuActions : MonoBehaviour
{
    // Variables de navegación
    private int arrowIndex; // Índice de la posición actual del selector

    // Referencias a objetos UI
    [SerializeField] GameObject Title, DK, Start, Selection; // Elementos del menú
    [SerializeField] InputActionAsset inputActionMenu; // Acciones de entrada
    [SerializeField] RectTransform selectMode; // Transform del selector
    [SerializeField] RectTransform[] arrowPositions; // Posiciones posibles del selector
    [SerializeField] bool isGameOver; // Flag para pantalla de Game Over

    // Acciones de entrada
    public InputAction anyKey, menuKey, menuSelect; // Teclas para navegar y seleccionar

    // Estados del menú
    enum State
    {
        PRESSTART, // Pantalla inicial "Presiona cualquier tecla"
        MENU       // Menú principal navegable
    }

    State state = State.PRESSTART; // Estado actual

    private void Awake()
    {
        // Configuración inicial del sistema de entrada
        inputActionMenu.Enable();
        anyKey = inputActionMenu.FindActionMap("PressAnyKey").FindAction("AnyKeyButton");
        menuKey = inputActionMenu.FindActionMap("VerticalMenu").FindAction("MenuMovement");
        menuSelect = inputActionMenu.FindActionMap("VerticalMenu").FindAction("SelectionMenu");

        arrowIndex = 0; // Inicia en la primera opción

        // Configuración inicial según el tipo de menú
        if (isGameOver)
        {
            state = State.MENU; // Si es Game Over, va directo al menú
        }
        else
        {
            // Muestra elementos de la pantalla inicial
            DK.SetActive(true);
            Start.SetActive(true);
            Selection.SetActive(false);
        }
    }

    private void Update()
    {
        // Máquina de estados del menú
        switch (state)
        {
            case State.PRESSTART:
                if (!isGameOver) PressAnyKey(); // Espera input para continuar
                break;
            case State.MENU:
                PressKeysMenu(); // Maneja navegación del menú
                Selection.SetActive(true); // Muestra selector
                break;
        }
    }

    // Maneja la pantalla "Presiona cualquier tecla"
    void PressAnyKey()
    {
        if (anyKey.triggered)
        {
            DK.SetActive(false);
            Start.SetActive(false);
            state = State.MENU; // Cambia al menú navegable
        }
    }

    // Vuelve al menú principal desde ajustes
    public void PressExitSettings()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    // Controla la navegación y selección en el menú
    void PressKeysMenu()
    {
        // Movimiento vertical del selector
        if (menuKey.ReadValue<float>() < 0 && menuKey.triggered)
        {
            arrowIndex++; // Mover abajo
            if (arrowIndex > arrowPositions.Length - 1) arrowIndex = 0; // Circular
        }
        else if (menuKey.ReadValue<float>() > 0 && menuKey.triggered)
        {
            arrowIndex--; // Mover arriba
            if (arrowIndex < 0) arrowIndex = arrowPositions.Length - 1; // Circular
        }
        selectMode.position = arrowPositions[arrowIndex].position; // Actualiza posición

        // Lógica de selección
        if (menuSelect.triggered)
        {
            switch (arrowIndex)
            {
                case 0: // Nueva partida
                    SceneManager.LoadScene("Cutscene1", LoadSceneMode.Single);
                    GameManager.Instance.AddLife(3); // Restaura vidas
                    break;
                case 1: // Ajustes o volver al menú
                    SceneManager.LoadScene(isGameOver ? "Menu" : "Settings", LoadSceneMode.Single);
                    break;
                case 2: // Ver informe
                    SceneManager.LoadScene("Informe");
                    break;
            }
        }
    }
}