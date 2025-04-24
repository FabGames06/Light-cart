using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // R�f�rence � votre "chariot"
    private float offset=1f; // D�calage de la cam�ra par rapport au joueur

    void LateUpdate()
    {
        if (target != null)
        {
            // Met � jour la position de la cam�ra avec le d�calage
            transform.position = new Vector3(target.position.x, target.position.y+offset, transform.position.z);
        }
    }
}
