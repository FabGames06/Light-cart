using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrivee : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // si on arrive à la ligne d'arrivée, c'est gagné, on change de scène
        // obsolète, c'est géré pas chariot.cs
        if(collision!= null && collision.name=="Chariot")
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
