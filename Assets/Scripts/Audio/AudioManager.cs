using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;
    public static AudioManager instance;

    public enum Sounds
    {
        Coin,
        Boost,
        Destroy,
        YouWin,
        GameOver
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y'a plus d'une instance de AudioManager");
            return;
        }
        instance = this;
    }
}
