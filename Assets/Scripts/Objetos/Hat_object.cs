using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat_object : MonoBehaviour
{
    [SerializeField] int Score = 500;
    [SerializeField] Animator animator;
    [SerializeField] private HighScoreContoller highScoreController;
    float destroyDelay = 2.5f;
    string IsScored = "IsScored";

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool(IsScored, false);
            StartCoroutine(DestroyAfterAnimation());
            highScoreController.AddScore(Score);
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
