using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrelController : MonoBehaviour
{
    [SerializeField] Animator fireAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrel"))
        {
            fireAnimator.gameObject.SetActive(true);
            fireAnimator.enabled = true;
        }
    }

    private void Awake()
    {
        fireAnimator.gameObject.SetActive(false);
    }
}
