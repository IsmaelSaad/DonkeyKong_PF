using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class High_Score_Contoller : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private int score;
    void Start()
    {
        scoreText.text = "Score: " + 0; 
    }


    void Update()
    {
        
    }

    public void AddScore(int ItemPoints)
    {
        score += ItemPoints;
        scoreText.text = "Score: " + score.ToString();
    }
}
