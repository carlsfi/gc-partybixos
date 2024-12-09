using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip buttonClip; // O mesmo áudio para todos os botões
    private AudioSource audioSource;

    void Start()
    {
        // Configurar o AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        if (buttonClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClip);
        }
    }
}
