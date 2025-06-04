using UnityEngine;
using System.Collections;

// Singleton que gestiona corrutines de forma global
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;  // Inst�ncia �nica del runner

    // Propietat per accedir a la inst�ncia (patr� Singleton)
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

    // M�tode per executar una corrutina
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);  // Inicia la corrutina
    }
}