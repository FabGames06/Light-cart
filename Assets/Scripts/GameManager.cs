using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int nbColonnes = 5;
    public int nbLignes = 10;
    public float offsetYPrefab; // = -3.2f;
    public float hauteurPrefab; // = 2.68f;

    private float pasIndex;
    private float offsetRailVert;

    public GameObject railVert;
    public GameObject railNE;
    public GameObject railNO;

    void Start()
    {
        // CREATION NIVEAU PROCEDURALE
        //float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        pasIndex = 1.7f; //screenWidth / 2f / nbColonnes;

        for(int b=0; b<nbLignes; b++)
            for(int a=0; a<nbColonnes; a++)
            {
                int prefabIndex = Random.Range(1, 3);
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
                // sécurité anti sortie de la zone avec 1ere et 5ème colonne (extrémitées)
                // 1ère colonne, on évite le rail NordOuest
                if(a==0)
                {
                    if (prefab == railNO)
                        prefab = railNE;
                }
                // 5ère colonne, on évite le rail NordEst
                if (a == nbColonnes-1)
                {
                    if (prefab == railNE)
                        prefab = railNO;
                }

                offsetRailVert = 0f;
                // décalage artificiel pour un rail de type rail_vertical
                if (prefab == railVert)
                    offsetRailVert = .3f;
                Vector3 posPrefab = new Vector3(-4f - offsetRailVert + pasIndex * a, offsetYPrefab + hauteurPrefab*b, 0f);
                Instantiate(prefab, posPrefab, Quaternion.identity);
            }
    }

}
