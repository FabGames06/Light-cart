using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Chariot : MonoBehaviour
{
    public float detectionDistance = .5f;
    public float offsetDetect = 0.5f;

    private bool isRouling;
    private bool lancement;
    private SplineAnimate currentSplineAnimate;

    void Start()
    {
        isRouling = false;
        lancement = false;

        currentSplineAnimate = GetComponent<SplineAnimate>();

        if (currentSplineAnimate == null)
            Debug.LogError("SplineAnimate introuvable sur le chariot !");
    }

    void Update()
    {
        if (!lancement)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.position = new Vector3(transform.position.x + 1.7f, transform.position.y);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.position = new Vector3(transform.position.x - 1.7f, transform.position.y);

            // controles des limites
            // limite droite
            if(transform.position.x > 2.54f)
                transform.position = new Vector3(2.54f, transform.position.y);
            // limite gauche
            if (transform.position.x < -4.26f)
                transform.position = new Vector3(-4.26f, transform.position.y);

            if(Input.GetKeyDown(KeyCode.Space))
                lancement = true;
        }

        // lancement de la main routine
        if (lancement)
        {
            if (!isRouling)
            {
                DetectCurrentRail();
            }
            else if(!currentSplineAnimate.IsPlaying)
            {
                Debug.Log("animation end");
                isRouling = false;
            }
        }
    }

    public void DetectCurrentRail()
    {
        Collider2D railHit = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);

        if (railHit != null)
        {
            Transform leParent = railHit.transform.parent;

            if (leParent == null)
                Debug.Log("leParent est null...");
            else
            {
                Debug.Log("railHit = " + railHit + "; prefab détecté : " + leParent);
                
                SplineContainer[] leSplineContainerTab = leParent.GetComponentsInChildren<SplineContainer>();
                Debug.Log("leSplineContainerTab[0]=" + leSplineContainerTab[0].name);

                SplineContainer leSplineContainer = leSplineContainerTab[0];

                if (leSplineContainerTab.Length>1)
                {
                    Debug.Log("leSplineContainerTab[1]=" + leSplineContainerTab[1].name);
                    leSplineContainer = leSplineContainerTab[1];
                    /*
                    // tourne à droite
                    if (leSplineContainerTab[1].name == "SplineRight" && Input.GetKey(KeyCode.RightArrow))
                    {
                        Debug.Log("touche droite appuyée !");
                        leSplineContainer = leSplineContainerTab[1];
                        // force sur le nouveau rail
                        currentSplineAnimate.Container = leSplineContainer; // Assigne le nouveau SplineContainer
                        currentSplineAnimate.Restart(false);
                        currentSplineAnimate.Play();
                    }

                    // tourne à gauche
                    if (leSplineContainerTab[1].name == "SplineLeft" && Input.GetKey(KeyCode.LeftArrow))
                    {
                        Debug.Log("touche gauche appuyée !");
                        leSplineContainer = leSplineContainerTab[1];
                    }
                    */
                }

                if (currentSplineAnimate != null && leSplineContainer != null && currentSplineAnimate.Container != leSplineContainer)
                {
                    currentSplineAnimate.Container = leSplineContainer; // Assigne le nouveau SplineContainer
                    currentSplineAnimate.Restart(false);
                    //Debug.Log("Nouveau SplineContainer assigné : " + currentSplineAnimate.Container);
                    currentSplineAnimate.Play();
                    isRouling = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun rail détecté, le chariot est en attente !");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);
    }
}
