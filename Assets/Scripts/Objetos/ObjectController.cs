using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

    [SerializeField] int scorePoints = 500;
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
        if (collision.CompareTag("Player"))
        {
            boxColl.enabled = false;
            gameManager.AddPoints(500);
            if (!isHammer)
            {
                animator.SetBool("IsScored", false);
                CoroutineRunner.Instance.StartCoroutine(DestroyAfterAnimation());
            }
            
            //objectPoints = GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = gameManager.GetPoints().ToString("D6");
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
