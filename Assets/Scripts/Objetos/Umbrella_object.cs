using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Umbrella_object : MonoBehaviour
{
    [SerializeField] int Score;
    [SerializeField] Animator animator;
    float destroyDelay = 2.5f;
    string IsScored = "IsScored";

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool(IsScored, false);
            StartCoroutine(DestroyAfterAnimation());
        }
    }
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
