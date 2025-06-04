using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// Controlador per a la creació i configuració d'escaleres al joc
public class EscalerasController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float escaleraLength; // Longitud de l'escala (configurable des de l'editor)

    [SerializeField]
    public GameObject escalera; // Prefab de l'escala

    [SerializeField]
    public SpriteRenderer escaleraSprite; // Component visual de l'escala

    [SerializeField]
    public BoxCollider2D entrar, salir, middle, sizeCollider; // Col·liders per diferents parts de l'escala

    // Inicialització abans del primer frame
    void Awake()
    {
        CreateEscaleras(); // Crea i configura l'escala
    }

    // Mètode que s'executa quan es modifiquen valors a l'editor
    void OnValidate()
    {
        float stairHeight = (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit);

        if (sizeCollider != null)
        {
            if (escaleraSprite != null && entrar != null && salir != null)
            {
                // Actualitza la mida i posició del col·lider principal
                sizeCollider.offset = new Vector3(sizeCollider.offset.x, ((stairHeight * escaleraLength) / 2) - stairHeight / 2, 0);
                sizeCollider.size = new Vector3(sizeCollider.size.x, (stairHeight * escaleraLength), 0);
            }
        }
    }

    // Crea i configura una nova escala
    public void CreateEscaleras()
    {
        float stairHeight = (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit);

        // Configura col·lider d'entrada
        entrar.offset = new Vector3(entrar.offset.x, -0.05f, 0);

        // Configura col·lider de sortida
        salir.offset = new Vector3(entrar.offset.x, ((stairHeight * escaleraLength)) - stairHeight / 4, 0);

        /* Codi de debug per visualitzar les escales
        for (int i = 0; i < escaleraLength; i++)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit) * i, 1);
            Instantiate(escalera, position, Quaternion.identity, transform);
        }*/

        // Configura col·lider central
        middle.offset = new Vector3(middle.offset.x, (stairHeight * escaleraLength) / 2 - 0.05f);
        middle.size = new Vector3(middle.size.x, stairHeight * escaleraLength - 0.08f);
    }
}