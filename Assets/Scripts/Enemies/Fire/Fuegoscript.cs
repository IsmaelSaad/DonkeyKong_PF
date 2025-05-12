using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Fuegoscript : MonoBehaviour
{
    private float LatestDirectionChangeTime;
    private float Speed = 3f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    public Enemy Fire;

    void calculatenewmovementvector()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), 0);
        movementPerSecond = movementDirection * Speed;
    }

    void Update()
    {
        
    }


}
