using UnityEngine;
using UnityEngine.UI;

// Gestor del sistema de sonido del juego
public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; // Control deslizante para ajustar el volumen

    void Start()
    {
        // Inicialización del volumen
        if (!PlayerPrefs.HasKey("musicVolume")) // Si no existe configuración previa
        {
            PlayerPrefs.SetFloat("musicVolume", 2); // Valor por defecto (máximo)
            Load(); // Carga la configuración
        }
        else
        {
            Load(); // Carga la configuración existente
        }
    }

    // Método para cambiar el volumen (llamado desde el UI)
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value; // Aplica el volumen globalmente
        Debug.Log(volumeSlider.value); // Log para depuración

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