using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    [SerializeField]
    LayerMask floorLayer;
    [SerializeField]
    public InputActionAsset inputActionMapping;
    [SerializeField]
    float speed, jumpSpeed;

    Animator animator;
    Rigidbody2D rb;

    Player mario;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        mario = new Player(speed, jumpSpeed, rb, animator, inputActionMapping);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
