using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrivee : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // si on arrive � la ligne d'arriv�e, c'est gagn�, on change de sc�ne
        if(collision!= null && collision.name=="Chariot")
            SceneManager.LoadScene("WinScene");
    }
}
