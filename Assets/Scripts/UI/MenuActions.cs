using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

// Controlador de las acciones y navegaci�n en los men�s del juego
public class MenuActions : MonoBehaviour
{
    // Variables de navegaci�n
    private int arrowIndex; // �ndice de la posici�n actual del selector

    // Referencias a objetos UI
    [SerializeField] GameObject Title, DK, Start, Selection; // Elementos del men�
    [SerializeField] InputActionAsset inputActionMenu; // Acciones de entrada
    [SerializeField] RectTransform selectMode; // Transform del selector
    [SerializeField] RectTransform[] arrowPositions; // Posiciones posibles del selector
    [SerializeField] bool isGameOver; // Flag para pantalla de Game Over

    // Acciones de entrada
    public InputAction anyKey, menuKey, menuSelect; // Teclas para navegar y seleccionar

    // Estados del men�
    enum State
    {
        PRESSTART, // Pantalla inicial "Presiona cualquier tecla"
        MENU       // Men� principal navegable
    }

    State state = State.PRESSTART; // Estado actual

    private void Awake()
    {
        // Configuraci�n inicial del sistema de entrada
        inputActionMenu.Enable();
        anyKey = inputActionMenu.FindActionMap("PressAnyKey").FindAction("AnyKeyButton");
        menuKey = inputActionMenu.FindActionMap("VerticalMenu").FindAction("MenuMovement");
        menuSelect = inputActionMenu.FindActionMap("VerticalMenu").FindAction("SelectionMenu");

        arrowIndex = 0; // Inicia en la primera opci�n

        // Configuraci�n inicial seg�n el tipo de men�
        if (isGameOver)
        {
            state = State.MENU; // Si es Game Over, va directo al men�
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
        // M�quina de estados del men�
        switch (state)
        {
            case State.PRESSTART:
                if (!isGameOver) PressAnyKey(); // Espera input para continuar
                break;
            case State.MENU:
                PressKeysMenu(); // Maneja navegaci�n del men�
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
            state = State.MENU; // Cambia al men� navegable
        }
    }

    // Vuelve al men� principal desde ajustes
    public void PressExitSettings()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    // Controla la navegaci�n y selecci�n en el men�
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
        selectMode.position = arrowPositions[arrowIndex].position; // Actualiza posici�n

        // L�gica de selecci�n
        if (menuSelect.triggered)
        {
            switch (arrowIndex)
            {
                case 0: // Nueva partida
                    SceneManager.LoadScene("Cutscene1", LoadSceneMode.Single);
                    GameManager.Instance.AddLife(3); // Restaura vidas
                    break;
                case 1: // Ajustes o volver al men�
                    SceneManager.LoadScene(isGameOver ? "Menu" : "Settings", LoadSceneMode.Single);
                    break;
                case 2: // Ver informe
                    SceneManager.LoadScene("Informe");
                    break;
            }
        }
    }
}