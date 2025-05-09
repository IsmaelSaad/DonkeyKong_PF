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

    }

    static void AddPoints(int p) {
        points += p;
    }

    static int ReturnPoints() {
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
