using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask floorLayer, stairsLayer;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] InputActionAsset inputActionMapping;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] Collider2D detectFloor;
    [SerializeField] Collider2D capsuleCollider;

    private Rigidbody2D rb;
    private Animator animator;
    
    public Player mario;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mario = new Player(transform, raycastOrigin, speed, jumpSpeed, rb, animator, inputActionMapping, floorLayer, capsuleCollider);
        mario.WakePlayer();
    }

    void Start()
    {
        mario.StartPlayer();
    }

    void Update()
    {
        mario.UpdatePlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mario.BarrilCollisionEnter(collision);
        mario.SueloCollisionEnter(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (detectFloor.IsTouching(collision)) 
        {
            mario.EscalerasCollisionEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mario.EscalerasCollisionExit(collision);
    }

    public Player GetMario()
    {
        return mario;
    }
}