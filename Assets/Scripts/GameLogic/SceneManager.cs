using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gestor de canvis d'escena i dades del joc
public class SignalManager : MonoBehaviour
{
    // Mètode per canviar d'escena amb gestió de dades
    public void ChangeCutsceneTo(string scene)
    {
        // Si estem a l'escena de selecció de nom
        if (SceneManager.GetActiveScene().name == "NamePlayer")
        {
            // Guarda el nom del jugador introduït
            MongoDBManager.Instance.playerData.name = GameObject.FindGameObjectWithTag("Text").GetComponent<TMP_Text>().text;
            MongoDBManager.Instance.playerConfig.name = GameObject.FindGameObjectWithTag("Text").GetComponent<TMP_Text>().text;
        }
        // Si estem a l'escena de diàleg final
        else if (SceneManager.GetActiveScene().name == "DialogoFinal")
        {
            // Calcula el temps de joc (en segons)
            MongoDBManager.Instance.playerData.playTime = (double)System.DateTime.Now.Second - MongoDBManager.Instance.playerData.playTime;

            // Guarda totes les dades a MongoDB
            MongoDBManager.Instance.SaveUserData(MongoDBManager.Instance.playerData);
            MongoDBManager.Instance.SaveConfigData(MongoDBManager.Instance.playerConfig);
            MongoDBManager.Instance.SaveHighScoreData(MongoDBManager.Instance.playerHighscore);
        }

        // Carrega la nova escena
        SceneManager.LoadScene(scene);
    }
}