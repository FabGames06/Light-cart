using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // OBSOLETE, PAS PU TROUVER DE SHADER QUI GERE LE DéFILEMENT...
    public float scrollSpeed = 0.2f; // Vitesse de défilement vertical
    private Material backgroundMaterial;

    void Start()
    {
        // Récupérer le matériau du SpriteRenderer
        backgroundMaterial = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        // Calcul de l'offset uniquement sur l'axe Y
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);

        // Appliquer l'offset au matériau
        backgroundMaterial.SetTextureOffset("_MainTex", offset);
    }
}
