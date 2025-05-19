using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalManager : MonoBehaviour
{
    public void ChangeCutsceneTo(string scene)
    {
        if (SceneManager.GetActiveScene().name == "NamePlayer") 
        {
            MongoDBManager.Instance.playerData.name = GameObject.FindGameObjectWithTag("Text").GetComponent<TMP_Text>().text;
        }
        else if (SceneManager.GetActiveScene().name == "DialogoFinal")
        {
            MongoDBManager.Instance.playerData.playTime = (double) System.DateTime.Now.Second - MongoDBManager.Instance.playerData.playTime;
            MongoDBManager.Instance.SaveUserData(MongoDBManager.Instance.playerData);
        }

        SceneManager.LoadScene(scene);
    }
}
