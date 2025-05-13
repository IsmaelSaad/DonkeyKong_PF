using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    protected Animator animator;
    protected float speed;

    public Enemy(Animator animator, float speed) {
        this.animator = animator;
        this.speed = speed;
    }

    virtual public void EnemyStart()
    {
        
    }

    virtual public void EnemyUpdate()
    {
        
    }
}
