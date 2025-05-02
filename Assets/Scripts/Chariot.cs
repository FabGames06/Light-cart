using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class Chariot : MonoBehaviour
{
    public float detectionDistance = .5f;
    public float offsetDetect = 0.5f;
    public GameObject canvastre;
    //public GameObject canvasGameOver;
    public GameObject canvasHUD;
    public GameObject explosion;

    private bool isRouling;
    private bool lancement;
    private SplineAnimate currentSplineAnimate;
    private int nbCoins;

    void Start()
    {
        isRouling = false;
        lancement = false;
        //canvasGameOver.SetActive(false);
        nbCoins = 0;

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
            {
                canvastre.SetActive(false);
                lancement = true;
            }
        }
        else
        {
            // lancement de la main routine
            if (!isRouling)
            {
                DetectCurrentRail();
            }
            else if (!currentSplineAnimate.IsPlaying)
            {
                Debug.Log("animation end");
                isRouling = false;
            }
        }
    }

    public void DetectCurrentRail()
    {
        Collider2D railHit=null;
        Collider2D[] railHitTab = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);

        // classement des name de collider par ordre alphab�tique pour avoir le "Sp" de Spline en dernier (les autres seront avant)
        //railHitTab = railHitTab.OrderBy(c => c.name).ToArray();
        // new : tri par valeur retourn�e
        railHit = railHitTab.FirstOrDefault(c => c.gameObject.name == "S_vertical");

        for (int i = 0;i < railHitTab.Length;i++)
            Debug.Log("railHitTab[" + i + "] = " + railHitTab[i].name);

        if (railHit != null)
        {
            Debug.Log("railHit = " + railHit + ";");

            // game over si on heurte une barri�re
            if (Array.Exists(railHitTab, element => element.name == "Barriere" || element.name == "Barriere(Clone)"))
            {
                Debug.Log("GAME OVER !");
                lancement = false;
                //transform.gameObject.SetActive(false);
                //canvasGameOver.SetActive(true);
                StartCoroutine(Explose());
                //SceneManager.LoadScene("GameOverScene");
            }

            Transform leParent = railHit.gameObject.transform.parent;

            if (leParent == null)
                Debug.Log("leParent est null...");
            else
            {
                Debug.Log("leParent d�tect� : " + leParent);

                SplineContainer[] leSplineContainerTab = leParent.GetComponentsInChildren<SplineContainer>();
                //Debug.Log("leSplineContainerTab[0]=" + leSplineContainerTab[0].name);

                // tri du tableau par ordre alphab�tique pour avoir SplineStraight en dernier par d�faut
                leSplineContainerTab= leSplineContainerTab.OrderBy(element => element.name).ToArray();
                
                for (int i = 0; i < leSplineContainerTab.Length; i++)
                    Debug.Log("leSplineContainerTab[" + i + "] = " + leSplineContainerTab[i].name);

                SplineContainer leSplineContainer = leSplineContainerTab[0];

                // essai de donner le controle
                if(Input.GetKey(KeyCode.UpArrow) && leSplineContainerTab.Length > 1)
                {
                    Debug.Log("touche Up appuy�e");
                    leSplineContainer = leSplineContainerTab[1];
                }

                /*
                if (leSplineContainerTab.Length > 1)
                {
                    //Debug.Log("leSplineContainerTab[1]=" + leSplineContainerTab[1].name);
                    //leSplineContainer = leSplineContainerTab[1];
                    
                    // tourne � droite
                    if (leSplineContainerTab[1].name == "SplineRight" && Input.GetKey(KeyCode.RightArrow))
                    {
                        Debug.Log("touche droite appuy�e !");
                        leSplineContainer = leSplineContainerTab[1];
                        // force sur le nouveau rail
                        currentSplineAnimate.Container = leSplineContainer; // Assigne le nouveau SplineContainer
                        currentSplineAnimate.Restart(false);
                        currentSplineAnimate.Play();
                    }

                    // tourne � gauche
                    if (leSplineContainerTab[1].name == "SplineLeft" && Input.GetKey(KeyCode.LeftArrow))
                    {
                        Debug.Log("touche gauche appuy�e !");
                        leSplineContainer = leSplineContainerTab[1];
                    }                    
                }
                */

                if (currentSplineAnimate != null && leSplineContainer != null && currentSplineAnimate.Container != leSplineContainer)
                {
                    currentSplineAnimate.Container = leSplineContainer; // Assigne le nouveau SplineContainer
                    currentSplineAnimate.Restart(false);
                    //Debug.Log("Nouveau SplineContainer assign� : " + currentSplineAnimate.Container);
                    currentSplineAnimate.Play();
                    isRouling = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun rail d�tect�, le chariot est en attente !");
            lancement = false;
            //SceneManager.LoadScene("WinScene");
        }
    }

    IEnumerator Explose()
    {
        // r�cup�ration des coords du chariot au moment de l'impact
        Transform ChariotTrans = transform;

        Vector3[] positions = new Vector3[]
        {
            new (ChariotTrans.position.x, ChariotTrans.position.y + .4f),
            new (ChariotTrans.position.x - .2f, ChariotTrans.position.y + .2f),
            new (ChariotTrans.position.x, ChariotTrans.position.y + .1f),
            new (ChariotTrans.position.x + .2f, ChariotTrans.position.y + .2f)
        };

        // on fait disparaitre le chariot avant les explosions
        transform.gameObject.SetActive(false);

        foreach (Vector3 pos in positions)
        {
            GameObject explosionInstance = Instantiate(explosion, pos, Quaternion.identity);

            // Attendre avant de d�truire cette explosion
            yield return new WaitForSeconds(0.5f);
            Destroy(explosionInstance);
        }

        /*
        Animator animator = explosion.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Pas d'Animator sur le prefab !");
            yield break;
        }

        // Attendre que l'animation active soit termin�e
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0)
        );
        */

        SceneManager.LoadScene("GameOverScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.name == "Coin" || collision.gameObject.name == "Coin(Clone)"))
        {
            collision.gameObject.SetActive(false);
            nbCoins++;
            canvasHUD.GetComponentInChildren<TextMeshProUGUI>().text = "x "+nbCoins.ToString();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);
    }
}
