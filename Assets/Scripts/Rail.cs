using UnityEngine;

public class Rail : MonoBehaviour
{
    [HideInInspector]
    public Transform[] railSegments; // Tableau des morceaux de rail

    void Awake()
    {
        // Récupérer tous les enfants SpriteRenderers comme segments de rail
        railSegments = GetComponentsInChildren<Transform>();
    }
}
