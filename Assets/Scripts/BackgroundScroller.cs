using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // OBSOLETE, PAS PU TROUVER DE SHADER QUI GERE LE D�FILEMENT...
    public float scrollSpeed = 0.2f; // Vitesse de d�filement vertical
    private Material backgroundMaterial;

    void Start()
    {
        // R�cup�rer le mat�riau du SpriteRenderer
        backgroundMaterial = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        // Calcul de l'offset uniquement sur l'axe Y
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);

        // Appliquer l'offset au mat�riau
        backgroundMaterial.SetTextureOffset("_MainTex", offset);
    }
}
