using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    int arrowIndex;

    [SerializeField] GameObject Title, DK, Start, Selection;
    [SerializeField] InputActionAsset inputActionMenu;
    [SerializeField] RectTransform selectMode;
    [SerializeField] RectTransform[] arrowPositions;
    [SerializeField] bool isGameOver;

    public InputAction anyKey, menuKey, menuSelect;

    enum State {
        PRESSTART, MENU
    }

    State state = State.PRESSTART;

    private void Awake()
    {
        inputActionMenu.Enable();
        anyKey = inputActionMenu.FindActionMap("PressAnyKey").FindAction("AnyKeyButton");
        menuKey = inputActionMenu.FindActionMap("VerticalMenu").FindAction("MenuMovement");
        menuSelect = inputActionMenu.FindActionMap("VerticalMenu").FindAction("SelectionMenu");

        arrowIndex = 0;

        if (isGameOver) state = State.MENU;
        else 
        {
            DK.SetActive(true);
            Start.SetActive(true);

            Selection.SetActive(false);
        }
    }

    private void Update()
    {
        switch (state) { 
            case State.PRESSTART:
                if (!isGameOver)
                {
                    PressAnyKey();
                }
                break;
            case State.MENU:
                PressKeysMenu();
                Selection.SetActive(true);
                break;
        }
    }

    void PressAnyKey ()
    {
        if (anyKey.triggered)
        {
            Debug.Log("asd");
            DK.SetActive(false);
            Start.SetActive(false);
            state = State.MENU;
        }
    }

    void PressKeysMenu ()
    {
        if (menuKey.ReadValue<float>() < 0 && menuKey.triggered) 
        {
            arrowIndex++;
            if (arrowIndex > arrowPositions.Length - 1)
            {
                arrowIndex = 0;
            }
        }
        else if(menuKey.ReadValue<float>() > 0 && menuKey.triggered)
        {
            arrowIndex--;
            if (arrowIndex < 0)
            {
                arrowIndex = arrowPositions.Length - 1;
            }
        }
        selectMode.position = arrowPositions[arrowIndex].position;

        if (menuSelect.triggered && arrowIndex == 0)
        {
            SceneManager.LoadScene("Lvl1");
        }
        else if (menuSelect.triggered && arrowIndex == 1) 
        {
            if (isGameOver)
            {
                SceneManager.LoadScene("Menu");
            }
            else 
            {
                // TODO
            }
                
        }
        //  if (isGameOver)
    }



}
    