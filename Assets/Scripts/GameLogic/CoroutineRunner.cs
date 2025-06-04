using UnityEngine;
using System.Collections;

// Singleton que gestiona corrutines de forma global
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;  // Instància única del runner

    // Propietat per accedir a la instància (patró Singleton)
    public static CoroutineRunner Instance
    {
        get
        {
            // Si no existeix, la crea
            if (_instance == null)
            {
                GameObject go = new GameObject("CoroutineRunner");  // Crea nou GameObject
                DontDestroyOnLoad(go);  // Persisteix entre escenes
                _instance = go.AddComponent<CoroutineRunner>();  // Afegeix el component
            }
            return _instance;
        }
    }

    // Mètode per executar una corrutina
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);  // Inicia la corrutina
    }
}