using System.Collections;
using UnityEngine;

public class Chariot : MonoBehaviour
{
    public float speed = 1f; // Vitesse du chariot
    public float detectionRadius = .3f; // Rayon pour d�tecter les rails courb�s ou intersections
    private bool isMovingUp = true; // Indique si le chariot est en mouvement
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    void Update()
    {
        if (isMovingUp)
        {
            // D�placer le chariot vers le haut
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (isMovingLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (isMovingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        //foreach(Collider2D obj in detectedObjects)
        //Debug.Log("longueur de Collider2D[] : "+ detectedObjects.Length);

        // on donne la possibilit� au chariot de tourner si on a une portion de rail NE ou NO
        if (collision.name=="S_NE" || collision.name == "S_NO")
        {
            //isMovingUp = false;
            Debug.Log("bifurcation sur : "+ collision.name);

            if (Input.GetKey(KeyCode.RightArrow) && collision.name == "S_NE")
            {
                isMovingRight = true;
                StartCoroutine(ArreteAvancer());
            }

            if (Input.GetKey(KeyCode.LeftArrow) && collision.name == "S_NO")
            {
                isMovingLeft = true;
                StartCoroutine(ArreteAvancer());
            }

        }
    }

    public IEnumerator ArreteAvancer()
    {
        yield return new WaitForSeconds(.6f);
        isMovingUp = false;
    }
    private void OnDrawGizmos()
    {
        // Dessiner la zone de d�tection pour visualiser le rayon
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
