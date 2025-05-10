using System.IO;
using TMPro;
using UnityEngine;

public class LastScore : MonoBehaviour
{
    public TextMeshProUGUI champScore;
    private void Start()
    {
        // récupération du score par le fichier créé sur la scène précédente
        string cheminFichier = Path.Combine(Application.persistentDataPath, "temp.json");
        if (File.Exists(cheminFichier))
        {
            // lecture du score sauvegardé précédemment
            string json = File.ReadAllText(cheminFichier);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);

            // maj du text mesh pro
            champScore.text = "Score : " + data.score;
        }
    }
}
