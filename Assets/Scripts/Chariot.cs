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
    public GameObject canvasHUD;
    public GameObject explosion;
    public static Chariot Instance { get; private set; }

    private bool isRouling;
    private bool lancement;
    private SplineAnimate currentSplineAnimate;
    private int nbCoins;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // DontDestroyOnLoad pour qu'il persiste entre les scènes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        isRouling = false;
        lancement = false;
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

        // classement des name de collider par ordre alphabétique pour avoir le "Sp" de Spline en dernier (les autres seront avant)
        //railHitTab = railHitTab.OrderBy(c => c.name).ToArray();
        // new : tri par valeur retournée
        railHit = railHitTab.FirstOrDefault(c => c.gameObject.name == "S_vertical");

        for (int i = 0;i < railHitTab.Length;i++)
            Debug.Log("railHitTab[" + i + "] = " + railHitTab[i].name);

        if (railHit != null)
        {
            Debug.Log("railHit = " + railHit + ";");

            // game over si on heurte une barrière
            if (Array.Exists(railHitTab, element => element.name == "Barriere" || element.name == "Barriere(Clone)"))
            {
                Debug.Log("GAME OVER !");
                lancement = false;
                //transform.gameObject.SetActive(false);
                //canvasGameOver.SetActive(true);
                Explosions();
                //SceneManager.LoadScene("GameOverScene");
            }

            Transform leParent = railHit.gameObject.transform.parent;

            if (leParent == null)
                Debug.Log("leParent est null...");
            else
            {
                Debug.Log("leParent détecté : " + leParent);

                SplineContainer[] leSplineContainerTab = leParent.GetComponentsInChildren<SplineContainer>();
                //Debug.Log("leSplineContainerTab[0]=" + leSplineContainerTab[0].name);

                // tri du tableau par ordre alphabétique pour avoir SplineStraight en premier par défaut
                leSplineContainerTab= leSplineContainerTab.OrderByDescending(element => element.name).ToArray();
                
                for (int i = 0; i < leSplineContainerTab.Length; i++)
                    Debug.Log("leSplineContainerTab[" + i + "] = " + leSplineContainerTab[i].name);

                SplineContainer leSplineContainer = leSplineContainerTab[0];

                // essai de débloquer lors du bug "ligne droite"
                if(Input.GetKey(KeyCode.UpArrow) && leSplineContainerTab.Length > 1)
                {
                    Debug.Log("touche Up appuyée");
                    leSplineContainer = leSplineContainerTab[1];
                }

                /*
                if (leSplineContainerTab.Length > 1)
                {
                    //Debug.Log("leSplineContainerTab[1]=" + leSplineContainerTab[1].name);
                    //leSplineContainer = leSplineContainerTab[1];
                    
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
                }
                */

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
            lancement = false;
            //SceneManager.LoadScene("WinScene");
        }
    }

    public void Explosions()
    {
        // récupération des coords du chariot au moment de l'impact
        Transform ChariotTrans = transform;

        Vector3[] positions = new Vector3[4];
        positions[0] = new(ChariotTrans.position.x, ChariotTrans.position.y + 1.5f);
        positions[1] = new(ChariotTrans.position.x - .75f, ChariotTrans.position.y + .75f);
        positions[2] = new(ChariotTrans.position.x, ChariotTrans.position.y + .5f);
        positions[3] = new(ChariotTrans.position.x + .75f, ChariotTrans.position.y + .75f);

        // on fait disparaitre le chariot avant les explosions
        //transform.gameObject.SetActive(false); --> risque de désactiver ce script si on le met active = false, et du coup désactive la suite aussi
        lancement = false;

        StartCoroutine(Explose(positions[0], 1f));
        StartCoroutine(Explose(positions[1], 2f));
        StartCoroutine(Explose(positions[2], 1.5f));
        StartCoroutine(Explose(positions[3], .5f));

        //transform.gameObject.SetActive(false);
        SpriteRenderer sr = transform.gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        StartCoroutine(EndExplosions());
    }
    public IEnumerator Explose(Vector3 position, float temps)
    {
        GameObject explosionInstance = Instantiate(explosion, position, Quaternion.identity);
        Debug.Log("Explosion de " + position);
        yield return new WaitForSeconds(temps);
        Destroy(explosionInstance, temps);

        /*
        Animator animator = explosion.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Pas d'Animator sur le prefab !");
            yield break;
        }

        // Attendre que l'animation active soit terminée
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0)
        );
        */

        //SceneManager.LoadScene("GameOverScene");
    }

    public IEnumerator EndExplosions()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOverScene");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.name == "Coin" || collision.gameObject.name == "Coin(Clone)"))
        {
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
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
