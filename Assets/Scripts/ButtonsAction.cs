using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsAction : MonoBehaviour
{
    public TMP_InputField champPseudo;
    public void Index()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void Score()
    {
        int scoreIndex;

        // tester si infos.sco existe, si non, le créér
        string cheminFichier = Path.Combine(Application.persistentDataPath, "infos.sco");
        if (!File.Exists(cheminFichier))
        {
            File.WriteAllText(cheminFichier, "1");
            scoreIndex = 1;
        }
        else
        {
            int.TryParse(File.ReadAllText(cheminFichier), out scoreIndex);
            scoreIndex++;
            // maj du fichier en comptabilisant le nouvel index
            File.WriteAllText(cheminFichier, scoreIndex.ToString());
        }

        // mettre à jour temp.json et le renommer
        cheminFichier = Path.Combine(Application.persistentDataPath, "temp.json");
        if (File.Exists(cheminFichier))
        {
            // lecture du score sauvegardé précédemment
            string json = File.ReadAllText(cheminFichier);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            // maj de l'attribut pseudo avec le pseudo saisi
            data.pseudo = champPseudo.text;
            // sauvegarde du json
            cheminFichier = Path.Combine(Application.persistentDataPath, "score"+scoreIndex+".json");
            json = JsonUtility.ToJson(data);
            File.WriteAllText(cheminFichier, json);
            // suppression du fichier temp
            cheminFichier = Path.Combine(Application.persistentDataPath, "temp.json");
            File.Delete(cheminFichier);
        }
        SceneManager.LoadScene("HighScoreScene");
    }
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void HighScores()
    {
        SceneManager.LoadScene("HighScoreScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
