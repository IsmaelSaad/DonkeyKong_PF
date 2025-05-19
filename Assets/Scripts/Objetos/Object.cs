using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ObjectEnv
{
    private GameObject itself;
    private GameManager gameManager;
    private BoxCollider2D boxColl;
    private int scorePoints;
    private Animator animator;
    private float destroyDelay;
    private bool isGotten = false;
    public bool isHammer = false;

    public ObjectEnv(GameObject itself, int scorePoints, Animator animator, float destroyDelay, GameManager gameManager, BoxCollider2D boxColl) 
    {
        this.itself = itself;
        this.scorePoints = scorePoints;
        this.animator = animator;
        this.destroyDelay = destroyDelay;
        this.gameManager = gameManager;
    }

    public void ObjectOnTriggerEnter(Collider2D collision) {
        if (collision.CompareTag("Player") && !isGotten)
        {
            isGotten = true;
            if (!isHammer) {
                animator.SetBool("IsScored", true);
                CoroutineRunner.Instance.StartCoroutine(DestroyAfterAnimation());
            }
            gameManager.AddPoints(scorePoints);
            MongoDBManager.Instance.playerData.objectsPickedUp++;
            GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = gameManager.GetPoints().ToString("D6");
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Object.Destroy(itself);
    }
}
