using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que controla el comportamiento del fuego en el juego
public class Fire : Enemy
{
    private float latestDirectionChangeTime; // �ltimo momento en que cambi� de direcci�n
    private float directionChangeInterval;  // Intervalo entre cambios de direcci�n
    private float moveSpeed;               // Velocidad de movimiento
    private int currentDirection = 1;      // Direcci�n actual (1 = derecha, -1 = izquierda)
    private Rigidbody2D rb;                // Componente de f�sica 2D

    // Constructor: inicializa los par�metros del fuego
    public Fire(Animator animator, float speed, float directionChangeInterval, Rigidbody2D rb) : base(animator, speed)
    {
        this.rb = rb;
        this.directionChangeInterval = directionChangeInterval;
        this.moveSpeed = speed;
    }

    // Inicializaci�n del fuego
    public override void EnemyStart()
    {
        latestDirectionChangeTime = Time.time; // Establece el tiempo inicial
    }

    // Actualizaci�n del comportamiento del fuego cada frame
    public override void EnemyUpdate()
    {
        // Cambia de direcci�n si ha pasado el intervalo establecido
        if (Time.time - latestDirectionChangeTime > directionChangeInterval)
        {
            currentDirection *= -1; // Invierte la direcci�n
            latestDirectionChangeTime = Time.time; // Reinicia el contador
        }

        Move(); // Aplica el movimiento
    }

    // M�todo que aplica el movimiento al fuego
    private void Move()
    {
        Vector2 movement = new Vector2(currentDirection * moveSpeed, rb.velocity.y);
        rb.velocity = movement; // Actualiza la velocidad del Rigidbody2D
    }
}