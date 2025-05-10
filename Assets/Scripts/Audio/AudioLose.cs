using UnityEngine;

public class AudioLose : MonoBehaviour
{
    private AudioManager audioInstance;

    void Start()
    {
        // récupération de l'instance d'AudioManager
        audioInstance = AudioManager.instance;

        audioInstance.audioSource.clip = audioInstance.playlist[(int)AudioManager.Sounds.GameOver];
        audioInstance.audioSource.Play();
    }
}
