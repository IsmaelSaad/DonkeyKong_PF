using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controlador centralizado de sonido y música del juego
public class SoundController : MonoBehaviour
{
    public AudioSource sceneAudio;    // Fuente de audio principal
    public AudioClip[] clip;         // Array de clips de audio [0]Menu [1]GameOver [2-4]Diálogos [5-6]Niveles

    public static SoundController Instance; // Instancia singleton

    // Propiedad para acceso seguro a la instancia
    public static SoundController instance
    {
        get
        {
            if (Instance == null)
            {
                // Creación dinámica si no existe
                GameObject go = new GameObject("SoundManager");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<SoundController>();
            }
            return Instance; // Corregido: devolver Instance en lugar de instance
        }
    }

    void Start()
    {
        // Configuración del patrón singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
            sceneAudio = GetComponent<AudioSource>(); // Obtiene componente AudioSource
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    private void OnEnable()
    {
        // Suscribe el evento de cambio de escena
        SceneManager.sceneLoaded += LoadMusic;
    }

    // Carga y reproduce música según la escena actual
    public void LoadMusic(Scene scene, LoadSceneMode mode)
    {
        if (sceneAudio == null) return;

        switch (scene.name)
        {
            case "Menu":
                PlayTrack(0, true); // Música de menú (loop)
                break;
            case "GameOver":
                PlayTrack(1, false); // Sonido de game over (sin loop)
                break;
            case "DialogoContexto":
                PlayTrack(2, false); // Diálogo 1
                break;
            case "DialogoMedio":
                PlayTrack(3, false); // Diálogo 2
                break;
            case "DialogoFinal":
                PlayTrack(4, false); // Diálogo final
                break;
            case "Lvl1":
                PlayTrack(5, true); // Música nivel 1 (loop)
                break;
            case "Lvl2":
                PlayTrack(6, true); // Música nivel 2 (loop)
                break;
            default:
                sceneAudio.Stop(); // Detiene audio en escenas no configuradas
                break;
        }
    }

    // Método helper para reproducir pistas
    private void PlayTrack(int clipIndex, bool loop)
    {
        sceneAudio.Stop();
        sceneAudio.clip = clip[clipIndex];
        sceneAudio.loop = loop;
        sceneAudio.Play();
    }
}