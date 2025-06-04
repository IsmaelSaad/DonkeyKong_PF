using UnityEngine;
using UnityEngine.UI;

// Gestor del sistema de sonido del juego
public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; // Control deslizante para ajustar el volumen

    void Start()
    {
        // Inicializaci�n del volumen
        if (!PlayerPrefs.HasKey("musicVolume")) // Si no existe configuraci�n previa
        {
            PlayerPrefs.SetFloat("musicVolume", 2); // Valor por defecto (m�ximo)
            Load(); // Carga la configuraci�n
        }
        else
        {
            Load(); // Carga la configuraci�n existente
        }
    }

    // M�todo para cambiar el volumen (llamado desde el UI)
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value; // Aplica el volumen globalmente
        Debug.Log(volumeSlider.value); // Log para depuraci�n

        // Guarda el volumen en la base de datos (convertido a porcentaje 0-100)
        MongoDBManager.Instance.playerConfig.volumeUser = (int)((volumeSlider.value / 2f) * 100f);
        Save(); // Guarda localmente
    }

    // Carga el volumen guardado en PlayerPrefs
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    // Guarda el volumen actual en PlayerPrefs
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}