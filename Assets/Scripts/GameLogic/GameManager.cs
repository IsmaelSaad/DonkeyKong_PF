using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Instancia singleton del GameManager
    [SerializeField] InputActionAsset inputActionPause, inputActionName; // Acciones de entrada para pausa y nombre
    [SerializeField] GameObject pause, life1, life2; // Referencias a UI elements
    private InputAction pauseKey, enterNameKey; // Acciones de teclado

    string actualScene; // Escena actual del juego

    static int lifes = 3; // Vidas del jugador (estático)
    private static int points = 0; // Puntos del jugador (estático)

    // Estados posibles del juego
    enum State
    {
        INGAME, // Jugando normalmente
        INPAUSE // Juego en pausa
    }

    State state = State.INGAME; // Estado actual

    void Start()
    {
        MongoDBManager.Instance.GetHighestScore(); // Obtiene los mejores puntajes al inicio
    }

    // Configuración del patrón singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void Update()
    {
        MongoDBManager.Instance.playerData.playTime = DateTime.Now.Second; // Actualiza tiempo de juego

        if (lifes > 3) lifes = 3; // Limita vidas máximas

        // Comportamiento según la escena actual
        if (actualScene == "NamePlayer")
        {
            enterNameKey = inputActionName.FindActionMap("VerticalMenu").FindAction("SelectionMenu");

            if (enterNameKey.triggered) // Si se presiona la tecla asignada
            {
                SceneManager.LoadScene("Menu"); // Va al menú
            }
        }
        else if (actualScene == "Lvl1" || actualScene == "Lvl2") // Niveles de juego
        {
            // Actualiza UI de puntos con formato de 6 dígitos
            GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = GetPoints().ToString("D6");

            MongoDBManager.Instance.playerData.score = GetPoints(); // Guarda puntos en BD

            if (lifes == 0) // Game Over
            {
                SceneManager.LoadScene("GameOver");
                ResetPoints();
            }

            // Actualiza visualización de vidas
            switch (lifes)
            {
                case 2:
                    life1.GetComponent<CanvasGroup>().alpha = 0; // Oculta 1 vida
                    break;
                case 1:
                    life1.GetComponent<CanvasGroup>().alpha = 0;
                    life2.GetComponent<CanvasGroup>().alpha = 0; // Oculta 2 vidas
                    break;
            }

            // Manejo de pausa
            switch (state)
            {
                case State.INGAME:
                    if (pauseKey.triggered) // Pausa el juego
                    {
                        pause.GetComponent<CanvasGroup>().alpha = 1;
                        state = State.INPAUSE;
                    }
                    Time.timeScale = 1; // Tiempo normal
                    break;
                case State.INPAUSE:
                    if (pauseKey.triggered) // Reanuda el juego
                    {
                        pause.GetComponent<CanvasGroup>().alpha = 0;
                        state = State.INGAME;
                    }
                    Time.timeScale = 0; // Detiene el tiempo
                    break;
            }
        }
        else if (actualScene == "Informe") // Pantalla de resultados
        {
            // Muestra estadísticas del jugador
            GameObject.FindGameObjectWithTag("Nombre").GetComponent<TMP_Text>().text = "Nombre: " + MongoDBManager.Instance.playerData.name;
            GameObject.FindGameObjectWithTag("Tiempo").GetComponent<TMP_Text>().text = "Tiempo de juego: " + MongoDBManager.Instance.playerData.playTime.ToString();
            GameObject.FindGameObjectWithTag("Puntos").GetComponent<TMP_Text>().text = "Puntos: " + MongoDBManager.Instance.playerData.score.ToString();
            GameObject.FindGameObjectWithTag("BarrilesSaltados").GetComponent<TMP_Text>().text = "Barriles saltados: " + MongoDBManager.Instance.playerData.barrelsJumped.ToString();
            GameObject.FindGameObjectWithTag("Objetos").GetComponent<TMP_Text>().text = "objetos recogidos: " + MongoDBManager.Instance.playerData.objectsPickedUp.ToString();

            // Muestra top 3 de puntajes
            GameObject.FindGameObjectWithTag("HighScore1").GetComponent<TMP_Text>().text = DatabaseUser.highScoreName[0] + " - " + DatabaseUser.highScorePoints[0];
            GameObject.FindGameObjectWithTag("HighScore2").GetComponent<TMP_Text>().text = DatabaseUser.highScoreName[1] + " - " + DatabaseUser.highScorePoints[1];
            GameObject.FindGameObjectWithTag("HighScore3").GetComponent<TMP_Text>().text = DatabaseUser.highScoreName[2] + " - " + DatabaseUser.highScorePoints[2];

            // Actualiza datos de highscore
            for (int i = 0; i < 3; i++)
            {
                MongoDBManager.Instance.playerHighscore.name[i] = DatabaseUser.highScoreName[i];
                MongoDBManager.Instance.playerHighscore.score[i] = DatabaseUser.highScorePoints[i];
            }
        }
    }

    // Evento al cargar escena
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Configura elementos al cargar escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lvl1" || scene.name == "Lvl2") // Niveles de juego
        {
            pause = GameObject.FindGameObjectWithTag("Pause");
            life1 = GameObject.FindGameObjectWithTag("Life1");
            life2 = GameObject.FindGameObjectWithTag("Life2");

            pauseKey = inputActionPause.FindActionMap("InGameTools").FindAction("Pause");
            pause.GetComponent<CanvasGroup>().alpha = 0; // Oculta menú pausa inicialmente
        }

        actualScene = scene.name; // Actualiza escena actual
    }

    // Métodos de gestión de puntos
    public void AddPoints(int p)
    {
        points += p;
    }

    public int GetPoints()
    {
        return points;
    }

    static void ResetPoints()
    {
        points = 0;
    }

    // Métodos de gestión de vidas
    public static void ResetLifes()
    {
        lifes = 3;
    }

    public int GetLifes()
    {
        return lifes;
    }

    public void AddLife(int d)
    {
        lifes += d;
    }

    public void DecrementLife(int d)
    {
        lifes -= d;
    }
}