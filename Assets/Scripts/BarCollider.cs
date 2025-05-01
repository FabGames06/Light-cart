using UnityEngine;

public class BarCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.name == "Chariot")
            Debug.Log("GAME OVER !");
    }
}
