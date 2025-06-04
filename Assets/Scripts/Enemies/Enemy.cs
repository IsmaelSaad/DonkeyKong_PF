// Classe base pare de tots els enemics del joc
using UnityEngine;

public class Enemy
{
    // Components i variables comunes a tots els enemics
    protected Animator animator;  // Controlador d'animacions
    protected float speed;       // Velocitat de moviment

    // Constructor base per als enemics
    public Enemy(Animator animator, float speed)
    {
        this.animator = animator;
        this.speed = speed;
    }

    // M�tode virtual que s'executa al iniciar l'enemic
    virtual public void EnemyStart()
    {
        // Els fills implementaran la seva l�gica
    }

    // M�tode virtual que s'executa cada frame
    virtual public void EnemyUpdate()
    {
        // Els fills implementaran la seva l�gica
    }
}