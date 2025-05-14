using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MenuActions : MonoBehaviour
{
    [SerializeField] GameObject Title;
    [SerializeField] GameObject DK;
    [SerializeField] GameObject Start;
    [SerializeField] InputActionAsset inputActionMenu;

    public InputAction anyKey, menuKey;

    enum State {
        PRESSTART, MENU
    }

    State state = State.PRESSTART;

    private void Awake()
    {
        inputActionMenu.Enable();
        anyKey = inputActionMenu.FindActionMap("PressAnyKey").FindAction("AnyKeyButton");
        menuKey = inputActionMenu.FindActionMap("VerticalMenu").FindAction("MenuMovement");
    }

    private void Update()
    {
        switch (state) { 
            case State.PRESSTART:
                PressAnyKey();
                break;
            case State.MENU:
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
        if (menuKey.ReadValue<float>() ) {

        }
    }



}
    