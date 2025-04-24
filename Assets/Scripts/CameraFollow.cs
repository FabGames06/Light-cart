using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Référence à votre "chariot"
    private float offset=1f; // Décalage de la caméra par rapport au joueur

    void LateUpdate()
    {
        if (target != null)
        {
            // Met à jour la position de la caméra avec le décalage
            transform.position = new Vector3(target.position.x, target.position.y+offset, transform.position.z);
        }
    }
}
