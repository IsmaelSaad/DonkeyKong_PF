using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class EscalerasController : MonoBehaviour
{
    
    [Range(0, 10)]
    public int escaleraLength;
    [SerializeField]
    public GameObject escalera;
    [SerializeField]
    public SpriteRenderer escaleraSprite;
    [SerializeField]
    public BoxCollider2D entrar, salir, sizeCollider;

    // Start is called before the first frame update
    void Start()
    {
        CreateEscaleras();
    }

    void OnValidate()
    {
        float stairHeight = (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit);
        if (escaleraSprite != null && entrar != null && salir != null)
        {
            sizeCollider.offset = new Vector3(sizeCollider.offset.x, ((stairHeight * escaleraLength) / 2) - stairHeight/2, 0);
            sizeCollider.size = new Vector3(sizeCollider.size.x, (stairHeight * escaleraLength), 0);
        }
    }

    public void CreateEscaleras()
    {
        float stairHeight = (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit);

        entrar.offset = new Vector3(entrar.offset.x, +stairHeight - stairHeight / 2, 0);
        salir.offset = new Vector3(entrar.offset.x, (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit) * escaleraLength + stairHeight/2, 0);
        for (int i = 0; i < escaleraLength; i++)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + (escaleraSprite.sprite.rect.height / escaleraSprite.sprite.pixelsPerUnit) * i, 1);
            Instantiate(escalera, position, Quaternion.identity, transform);
        }
    }
}
