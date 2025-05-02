using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int nbColonnes = 5;
    public int nbLignes = 10;
    public int nbPieges = 10;
    public int nbPieces = 35;
    public float offsetYPrefab; // = -3.2f;
    public float hauteurPrefab; // = 2.68f;

    private float pasIndex;
    private float offsetRailVert;

    public GameObject railVert;
    public GameObject railNE;
    public GameObject railNO;
    public GameObject barriere;
    public GameObject coin;

    void Start()
    {
        // CREATION PROCEDURALE DU NIVEAU
        //float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        pasIndex = 1.7f; //screenWidth / 2f / nbColonnes;

        for(int b=0; b<nbLignes; b++)
            for(int a=0; a<nbColonnes; a++)
            {
                int prefabIndex = Random.Range(1, 4);
                //prefabIndex = 1;  // force uniquement des rails droits pour les tests
                GameObject prefab;
                switch(prefabIndex)
                {
                    case 1:
                        //Instantiate(railVert, posPrefab, Quaternion.identity);
                        prefab = railVert;
                    break;

                    case 2:
                        prefab = railNE;
                    break;

                    default:
                    case 3:
                        prefab = railNO;
                    break;
                }
                // s�curit� anti sortie de la zone avec 1ere et 5�me colonne (extr�mit�es)
                // 1�re colonne, on �vite le rail NordOuest
                if(a==0)
                {
                    if (prefab == railNO)
                        prefab = railNE;
                }
                // 5�re colonne, on �vite le rail NordEst
                if (a == nbColonnes-1)
                {
                    if (prefab == railNE)
                        prefab = railNO;
                }

                offsetRailVert = 0f;
                // d�calage artificiel pour un rail de type rail_vertical
                if (prefab == railVert)
                    offsetRailVert = .27f;
                Vector3 posPrefab = new Vector3(-4f - offsetRailVert + pasIndex * a, offsetYPrefab + hauteurPrefab*b, 0f);
                Instantiate(prefab, posPrefab, Quaternion.identity);
            }

        // AJOUT DES BARRIERES PIEGES
        for(int a=0;a<nbPieges;a++)
        {
            float abscisseRandom = Random.Range(1, nbColonnes+1);
            // transformation des coordonn�es (x)
            //abscisseRandom = -4f - offsetRailVert + pasIndex * abscisseRandom;

            switch(abscisseRandom)
            {
                case 1:
                    abscisseRandom = -4.29f;    // -2.93f;
                break;

                case 2:
                    abscisseRandom = -2.59f;    // -1.23f;
                break;

                case 3:
                    abscisseRandom = -0.89f; // 0.47f;
                break;

                case 4:
                    abscisseRandom = 0.81f;  // 2.17f;
                break;

                default:
                case 5:
                    abscisseRandom = 2.51f; // 3.87f;
                break;
            }

            float ordonneeRandom = Random.Range(1, nbLignes-1);
            // transformation des coordonn�es (y)
            ordonneeRandom = hauteurPrefab * ordonneeRandom;
            Instantiate(barriere, new Vector3(abscisseRandom, ordonneeRandom), Quaternion.identity);
        }

        // AJOUT DES (BONUS) PIECES (coin)
        for (int a = 0; a < nbPieces; a++)
        {
            // test si pas d�j� un coin � cet endroit pour �viter de les empiler
            Vector3 nouvellePosition;
            do
            {
                float abscisseRandom = Random.Range(1, nbColonnes + 1);

                switch (abscisseRandom)
                {
                    case 1:
                        abscisseRandom = -4.29f;    // -2.93f;
                        break;

                    case 2:
                        abscisseRandom = -2.59f;    // -1.23f;
                        break;

                    case 3:
                        abscisseRandom = -0.89f; // 0.47f;
                        break;

                    case 4:
                        abscisseRandom = 0.81f;  // 2.17f;
                        break;

                    default:
                    case 5:
                        abscisseRandom = 2.51f; // 3.87f;
                        break;
                }

                nouvellePosition = GenererPositionAleatoire(abscisseRandom, hauteurPrefab, nbLignes);
            } while (!PositionLibre(nouvellePosition));

            Instantiate(coin, nouvellePosition, Quaternion.identity);
        }
    }

    bool PositionLibre(Vector3 position)
    {
        Collider2D[] testCoins = Physics2D.OverlapCircleAll(position, 1f);

        foreach (Collider2D coin in testCoins)
        {
            if (coin.name == "Coin" || coin.name == "Coin(Clone)")
            {
                return false; // Une pi�ce est d�j� l�, donc l'emplacement n'est pas libre
            }
        }
        return true; // Aucune pi�ce trouv�e, l'emplacement est libre
    }


    Vector3 GenererPositionAleatoire(float abscisseRandom, float hauteurPrefab, int nbLignes)
    {
        float ordonneeRandom = Random.Range(1, nbLignes-1);
        ordonneeRandom = (hauteurPrefab - 1.5f) * ordonneeRandom;
        return new Vector3(abscisseRandom, ordonneeRandom);
    }


}
