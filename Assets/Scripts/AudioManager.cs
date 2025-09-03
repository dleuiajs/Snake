using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] AudioSource audioSource;

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, 0.5f);
    }
}
