using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    /*
        FindObjectOfType<PlayerController o GameManager>();
     */

    int lifes = 3;
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

    private void AddLife(int d)
    {
        lifes += d;
    }


    public void DecrementLife(int d)
    {
        lifes -= d;
        Debug.Log(lifes);
    }
}
