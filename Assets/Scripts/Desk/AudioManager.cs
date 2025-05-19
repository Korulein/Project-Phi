using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioPrefab;

    [Header("SFX Clips")]
    [SerializeField] public AudioClip cameraFlash;

    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {

        // Instantiates Audio Source
        AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Get Length of Audio Clip
        float clipLength = audioSource.clip.length;

        // Destroy Clip Object
        Destroy(audioSource.gameObject, clipLength);
    }
}
