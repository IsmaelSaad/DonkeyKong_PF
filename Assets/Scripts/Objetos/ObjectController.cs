using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

    [SerializeField] int scorePoints;
    [SerializeField] Animator animator;
    [SerializeField] float destroyDelay;
    [SerializeField] bool isHammer;

    string objectPoints;
    BoxCollider2D boxColl;
    GameManager gameManager;
    ObjectEnv obj;

    // Start is called before the first frame update
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        obj = new ObjectEnv(gameObject, scorePoints, animator, destroyDelay, gameManager, boxColl);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        obj.ObjectOnTriggerEnter(collision);
    }
}
