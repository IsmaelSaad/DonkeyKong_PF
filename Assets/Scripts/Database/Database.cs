// Conté totes les estructures de dades per guardar informació del joc
using System;

[Serializable]
public class DatabaseUser
{
    // Dades bàsiques de l'usuari
    public string name;          // Nom del jugador
    public int score;            // Punts totals
    public int barrelsJumped;    // Barrils esquivats
    public double playTime;      // Temps de joc acumulat
    public int objectsPickedUp;  // Objectes recollits

    // Arrays estàtics per guardar els rècords per la UI !!! -> NO PER LA BD 
    static public int[] highScorePoints;  // Punts dels millors jugadors
    static public string[] highScoreName; // Noms dels millors jugadors

    // Constructor que inicialitza tot a zero
    public DatabaseUser()
    {
        name = "";
        score = 0;
        barrelsJumped = 0;
        playTime = 0f;
        objectsPickedUp = 0;
        highScoreName = new string[3];   // Top 3 noms
        highScorePoints = new int[3];    // Top 3 puntuacions
    }
}

// Estructura per guardar els rècords alts
[Serializable]
public class DatabaseHighscore
{
    public string[] name;  // Noms dels jugadors amb rècord
    public int[] score;     // Puntuacions dels rècords

    public DatabaseHighscore()
    {
        name = new string[3];  // Top 3 noms
        score = new int[3];    // Top 3 puntuacions
    }
}

// Configuració de l'usuari
[Serializable]
public class DatabaseConfig
{
    public string name;         // Nom del jugador
    public int volumeUser;      // Volum del joc (0-100)

    public DatabaseConfig()
    {
        name = "";
        volumeUser = 100;  // Volum al màxim per defecte
    }
}