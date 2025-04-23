using UnityEngine;

public class Chariot : MonoBehaviour
{
    public float speed = 1f; // Vitesse du chariot
    public float detectionRadius = .3f; // Rayon pour d�tecter les rails courb�s ou intersections
    private bool isMoving = true; // Indique si le chariot est en mouvement

    void Update()
    {
        if (isMoving)
        {
            // D�placer le chariot vers le haut
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        //foreach(Collider2D obj in detectedObjects)

            if(collision.CompareTag("Rail"))
            {
                isMoving = false;
                Debug.Log("arret sur : "+ collision.name);
                Debug.Log("longueur de Collider2D[] : "+ detectedObjects.Length);
            }
    }

    private void OnDrawGizmos()
    {
        // Dessiner la zone de d�tection pour visualiser le rayon
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
