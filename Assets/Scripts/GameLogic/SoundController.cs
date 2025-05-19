using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public AudioSource sceneAudio;
    public AudioClip[] clip;
    private bool noAudio = false;

    public static SoundController Instance;
    public static SoundController instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("GameManager");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<SoundController>();
            }
            return instance;
        }
    }


    void Start()
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
        sceneAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadMusic;
    }

    public void LoadMusic(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Menu":
                sceneAudio.clip = clip[0];
                sceneAudio.loop = true;
                sceneAudio.Play();
                break;
            case "GameOver":
                sceneAudio.loop = false;
                sceneAudio.clip = clip[1];
                sceneAudio.Play();
                break;
            case "DialogoContexto":
                sceneAudio.loop = false;
                sceneAudio.clip = clip[2];
                sceneAudio.Play();
                break;
            case "DialogoMedio":
                sceneAudio.loop = false;
                sceneAudio.clip = clip[3];
                sceneAudio.Play();
                break;
            case "DialogoFinal":
                sceneAudio.loop = false;
                sceneAudio.clip = clip[4];
                sceneAudio.Play();
                break;
            case "Lvl1":
                sceneAudio.loop = true;
                sceneAudio.clip = clip[5];
                sceneAudio.Play();
                break;
            case "Lvl2":
                sceneAudio.loop = true;
                sceneAudio.clip = clip[6];
                sceneAudio.Play();
                break;
            default:
                sceneAudio.Stop();
                noAudio = true;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

        

    }
}
