using TMPro;
using Unity.VisualScripting;
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
    
    public static CoroutineRunner _instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("GameManager");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<GameManager>();
            }
            return _instance;
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
       

            if (lifes == 0)
            {
                Debug.Log("gameover");
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
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lvl1" || scene.name == "Lvl2")
        {
            pauseKey = inputActionPause.FindActionMap("InGameTools").FindAction("Pause");
            pause.GetComponent<CanvasGroup>().alpha = 0;

            pause = GameObject.FindGameObjectWithTag("Pause");
            life1 = GameObject.FindGameObjectWithTag("Life1");
            life2 = GameObject.FindGameObjectWithTag("Life2");
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
