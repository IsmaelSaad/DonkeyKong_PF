using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database 
{
    private bool startedGame = false;
    public string name;  
    public int score;
    public int barrelsJumped;
    public double playTime;
    public int objectsPickedUp;

    public Database() {
        name = string.Empty;
        score = 0;
        barrelsJumped = 0;
        playTime = 0f;
        objectsPickedUp = 0;
    }
}
