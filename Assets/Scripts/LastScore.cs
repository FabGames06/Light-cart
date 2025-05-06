using System.IO;
using TMPro;
using UnityEngine;

public class LastScore : MonoBehaviour
{
    public TextMeshProUGUI champScore;
    private void Start()
    {
        // r�cup�ration du score par le fichier cr�� sur la sc�ne pr�c�dente
        string cheminFichier = Path.Combine(Application.persistentDataPath, "temp.json");
        if (File.Exists(cheminFichier))
        {
            // lecture du score sauvegard� pr�c�demment
            string json = File.ReadAllText(cheminFichier);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);

            // maj du text mesh pro
            champScore.text = "Score : " + data.score;
        }
    }
}
