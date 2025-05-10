using UnityEngine;

public class AudioWin : MonoBehaviour
{
    private AudioManager audioInstance;

    void Start()
    {
        // r�cup�ration de l'instance d'AudioManager
        audioInstance = AudioManager.instance;

        audioInstance.audioSource.clip = audioInstance.playlist[(int)AudioManager.Sounds.YouWin];
        audioInstance.audioSource.Play();
    }
}
