using System;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Splines;

public class Chariot : MonoBehaviour
{
    public float detectionDistance = 1f;
    public float offsetDetect = 0.5f;
    public GameObject canvastre;
    public GameObject canvasHUD;
    public GameObject explosion;

    public SplineContainer[] leSplineContainerTab;
    public Spline currentSpline;
    public float t = 0f; // Progression sur la spline
    public float speed = 1f;
    private int nbCoins;

    public Camera mainCamera;
    private string cheminFichier;
    public enum Etat
    {
        Commence,
        Lance,
        Toutdroit,
        Deraille,
        Detruit
    }

    public Etat etatActuel;

    void Start()
    {
        etatActuel = Etat.Commence;
        nbCoins = 0;
        cheminFichier = Path.Combine(Application.persistentDataPath, "temp.json");

        // Assurer que le chariot est bien visible en 2D
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Forcer l'affichage sur la bonne couche
        GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
    }

    void Update()
    {
        Debug.Log($"Position du chariot : {transform.position}");

        // Vérification du suivi de la caméra
        if (mainCamera != null)
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        switch (etatActuel)
        {
            case Etat.Commence:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    transform.position += new Vector3(1.7f, 0f);

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    transform.position += new Vector3(-1.7f, 0f);

                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, -4.26f, 2.54f),
                    transform.position.y,
                    0);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    canvastre.SetActive(false);
                    etatActuel = Etat.Lance;
                }
            break;

            case Etat.Lance:
                currentSpline = DetectCurrentRail();
                t = 0f;
                etatActuel = Etat.Toutdroit;
            break;

            case Etat.Toutdroit:
                if (currentSpline != null)
                {
                    t += (speed * Time.deltaTime) / currentSpline.GetLength();
                    t = Mathf.Clamp(t, 0, 1);

                    // Vérifier si la spline est bien évaluée
                    Vector3 evaluatedPos = SplineUtility.EvaluatePosition(currentSpline, t);
                    if (float.IsNaN(evaluatedPos.x) || float.IsNaN(evaluatedPos.y))
                    {
                        Debug.LogError("Erreur : La position évaluée est NaN !");
                        return;
                    }

                    // Conversion en coordonnées globales
                    Vector3 worldPos = leSplineContainerTab[0].transform.TransformPoint(evaluatedPos);
                    transform.position = new Vector3(worldPos.x, worldPos.y, 0); // Correction Z

                    // Vérifier si la tangente est valide
                    Vector3 tangent = SplineUtility.EvaluateTangent(currentSpline, t);
                    if (float.IsNaN(tangent.x) || float.IsNaN(tangent.y))
                    {
                        Debug.LogError("Erreur : La tangente est NaN !");
                        return;
                    }

                    transform.rotation = Quaternion.LookRotation(Vector3.forward, tangent); // Orientation 2D

                    if (t >= 1f)
                        etatActuel = Etat.Lance;
                }
            break;


            case Etat.Detruit:
                Debug.Log("Lancement de l'explosion !");

                SauveScore();
                Explosions();
            break;
        }
    }

    public Spline DetectCurrentRail()
    {
        Collider2D[] railHitTab = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);
        Collider2D railHit = railHitTab.FirstOrDefault(c => c.gameObject.name == "S_vertical");

        if (railHit != null)
        {
            Transform leParent = railHit.transform.parent;
            if (leParent == null)
            {
                Debug.LogError("leParent est null...");
                return null;
            }

            leSplineContainerTab = leParent.GetComponentsInChildren<SplineContainer>()
                                            .OrderByDescending(c => c.name)
                                            .ToArray();

            if (leSplineContainerTab.Length == 0)
                return null;

            return etatActuel == Etat.Deraille ? leSplineContainerTab[1].Spline : leSplineContainerTab[0].Spline;
        }
        else
        {
            Debug.LogWarning("Aucun rail détecté, le chariot est en attente !");
            return null;
        }
    }


    public void Explosions()
    {
        // récupération des coords du chariot au moment de l'impact
        Transform ChariotTrans = transform;

        Vector3[] positions = new Vector3[4];
        positions[0] = new(ChariotTrans.position.x, ChariotTrans.position.y + 1.5f);
        positions[1] = new(ChariotTrans.position.x - .5f, ChariotTrans.position.y + .75f);
        positions[2] = new(ChariotTrans.position.x, ChariotTrans.position.y + .5f);
        positions[3] = new(ChariotTrans.position.x + .5f, ChariotTrans.position.y + .75f);

        // on fait disparaitre le chariot avant les explosions
        //transform.gameObject.SetActive(false); --> risque de désactiver ce script si on le met active = false, et du coup désactive la suite aussi
        //lancement = false;

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
    }

    public IEnumerator EndExplosions()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOverScene");
    }

    private void SauveScore()
    {
        // sauvegarde du score dans un fichier json avant le changement de scène
        // obigé de mettre un cast (int) bizarre vu que Math.Ceiling retourne déjà un entier (arrondi)
        ScoreData data = new ScoreData { score = (int)Math.Ceiling(nbCoins * speed) };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(cheminFichier, json);
    }

    private void RefreshScore()
    {
        canvasHUD.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "x " + nbCoins.ToString();
        canvasHUD.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "SCORE : " + Math.Ceiling(nbCoins * speed).ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // GESTION DES PIECES D'OR (COINS)
        if (collision != null && (collision.gameObject.name == "Coin" || collision.gameObject.name == "Coin(Clone)"))
        {
            Destroy(collision.gameObject);
            nbCoins++;
            RefreshScore();
        }

        // GESTION GAME OVER AVEC UNE BARRIERE
        if (collision != null && (collision.name == "Barriere" || collision.name == "Barriere(Clone)"))
        {
            Debug.Log("GAME OVER !");
            etatActuel = Etat.Detruit;
        }

        // BONUS BOOST ACCELERATEUR
        if (collision != null && (collision.name == "Boost" || collision.name == "Boost(Clone)"))
        {
            Destroy(collision.gameObject);
            speed+=.5f;
            // maj du score car la vitesse du boost est aussi un coef multiplicateur du score
            RefreshScore();
        }

        // VICTOIRE SI ON FRANCHI LA LIGNE D'ARRIVEE
        if (collision != null && collision.name == "LigneArrivee")
        {
            // sauvegarde du score
            SauveScore();
            // changement de scène
            SceneManager.LoadScene("WinScene");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + offsetDetect), detectionDistance);
    }
}
