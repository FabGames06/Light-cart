using UnityEngine;
using System.IO;
using System;
using TMPro;

public class HighScores : MonoBehaviour
{
    public TextMeshProUGUI scoreTab;
    private int nbScores;
    private ScoreData[] scoreData;

    void Start()
    {
        // refresh du TMP
        scoreTab.ForceMeshUpdate();
        // ouverture infos.sco
        string cheminFichier = Path.Combine(Application.persistentDataPath, "infos.sco");
        if (File.Exists(cheminFichier))
        {
            int.TryParse(File.ReadAllText(cheminFichier), out nbScores);
            // limiter � 8 ? -> plus loin
            // et s'il y'en a moins que 8 ? le nb ds le fichier g�re ce max
            // et + que 8 ? idem, mais plus loin �a filtrera que les 8 premiers
            scoreData = new ScoreData[nbScores];
        }

        // r�cup des scores + pseudos en tableau
        for(int i = 1; i<=nbScores; i++)
        {
            cheminFichier = Path.Combine(Application.persistentDataPath, "score"+i+".json");
            string json = File.ReadAllText(cheminFichier);

            scoreData[i-1] = JsonUtility.FromJson<ScoreData>(json);
        }
        // tri du tableau pour scores d�croissants
        Array.Sort(scoreData, (a, b) => b.score.CompareTo(a.score));

        // maj du text mesh pro avec les scores class�s + syst�me de completion des "........" pour tjrs matcher � 35 caract�res
        scoreTab.text = "";

        // limitation aux 10 premiers
        if(nbScores>10)
            nbScores = 10;

        for (int i = 1; i <= nbScores; i++)
        {
            int nbPoints = 30 - scoreData[i - 1].pseudo.Length - scoreData[i - 1].score.ToString().Length;
            scoreTab.text += scoreData[i - 1].pseudo;
            for (int a = 0; a < nbPoints; a++)
                scoreTab.text += "_";
            scoreTab.text += scoreData[i-1].score +"\n";
        }
            
    }
}
