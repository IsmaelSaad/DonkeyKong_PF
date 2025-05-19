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
    public static GameManager Instance;
    [SerializeField] InputActionAsset inputActionPause, inputActionName;
    [SerializeField] GameObject pause, life1, life2;
    private InputAction pauseKey, enterNameKey;

    string actualScene;
    
    public static GameManager instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("GameManager");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    /*
        FindObjectOfType<PlayerController o GameManager>();
     */

    static int lifes = 3;
    private static int points = 0;
    private static string textPoints;

    enum State
    {
        INGAME, INPAUSE
    }

    State state = State.INGAME;

    void Start()
    {
        MongoDBManager.Instance.GetHighestScore();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        MongoDBManager.Instance.playerData.playTime = DateTime.Now.Second;

        Debug.Log(MongoDBManager.Instance.playerData.name);

        if (lifes > 3) lifes = 3;

        if (actualScene == "NamePlayer")
        {
            enterNameKey = inputActionName.FindActionMap("VerticalMenu").FindAction("SelectionMenu");

            if (enterNameKey.triggered)
            {
                SceneManager.LoadScene("Menu");
            }
        }

        else if (actualScene == "Lvl1" || actualScene == "Lvl2")
        {
            GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = GetPoints().ToString("D6");

            MongoDBManager.Instance.playerData.score = GetPoints();

            if (lifes == 0)
            {
                SceneManager.LoadScene("GameOver");
                ResetPoints();
            }

            switch (lifes)
            {
                case 2:
                    life1.GetComponent<CanvasGroup>().alpha = 0;
                    break;
                case 1:
                    life1.GetComponent<CanvasGroup>().alpha = 0;
                    life2.GetComponent<CanvasGroup>().alpha = 0;
                    break;
            }

            switch (state)
            {
                case State.INGAME:
                    if (pauseKey.triggered)
                    {
                        pause.GetComponent<CanvasGroup>().alpha = 1;
                        state = State.INPAUSE;
                    }
                    Time.timeScale = 1;
                    break;
                case State.INPAUSE:
                    if (pauseKey.triggered)
                    {
                        pause.GetComponent<CanvasGroup>().alpha = 0;
                        state = State.INGAME;
                    }
                    Time.timeScale = 0;
                    break;
            }
        }
        else if (actualScene == "Informe")
        {
            GameObject.FindGameObjectWithTag("Nombre").GetComponent<TMP_Text>().text = "Nombre: " + MongoDBManager.Instance.playerData.name;

            GameObject.FindGameObjectWithTag("Tiempo").GetComponent<TMP_Text>().text = "Tiempo de juego: " + MongoDBManager.Instance.playerData.playTime.ToString();

            GameObject.FindGameObjectWithTag("Puntos").GetComponent<TMP_Text>().text = "Puntos: " + MongoDBManager.Instance.playerData.score.ToString();

            GameObject.FindGameObjectWithTag("BarrilesSaltados").GetComponent<TMP_Text>().text = "Barriles saltados: " + MongoDBManager.Instance.playerData.barrelsJumped.ToString();

            GameObject.FindGameObjectWithTag("Objetos").GetComponent<TMP_Text>().text = "objetos recogidos: " + MongoDBManager.Instance.playerData.objectsPickedUp.ToString();


            GameObject.FindGameObjectWithTag("HighScore1").GetComponent<TMP_Text>().text = Database.highScoreName[0] + " - " + Database.highScorePoints[0];
            GameObject.FindGameObjectWithTag("HighScore2").GetComponent<TMP_Text>().text = Database.highScoreName[1] + " - " + Database.highScorePoints[1];
            GameObject.FindGameObjectWithTag("HighScore3").GetComponent<TMP_Text>().text = Database.highScoreName[2] + " - " + Database.highScorePoints[2];



        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lvl1" || scene.name == "Lvl2")
        {
            pause = GameObject.FindGameObjectWithTag("Pause");
            life1 = GameObject.FindGameObjectWithTag("Life1");
            life2 = GameObject.FindGameObjectWithTag("Life2");

            pauseKey = inputActionPause.FindActionMap("InGameTools").FindAction("Pause");
            pause.GetComponent<CanvasGroup>().alpha = 0;
        }

        actualScene = scene.name;
    }

    public void AddPoints(int p) {
        points += p;
    }

    public int GetPoints() {
        return points;
    }

    static void ResetPoints()
    {
        points = 0;
    }

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
