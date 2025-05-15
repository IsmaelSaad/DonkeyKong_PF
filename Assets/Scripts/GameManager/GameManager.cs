using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>().text = GetPoints().ToString("D6");

        if (lifes == 0)
        {
            Debug.Log("gameover");
            SceneManager.LoadScene("GameOver");
        }
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
        Debug.Log(lifes);
    }
}
