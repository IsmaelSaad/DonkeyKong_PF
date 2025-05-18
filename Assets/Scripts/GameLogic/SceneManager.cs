using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalManager : MonoBehaviour
{
    public void ChangeCutsceneTo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
