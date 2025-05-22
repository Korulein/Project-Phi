using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioPrefab;

    [Header("SoundFXClips")]
    public AudioClip cameraFlash;
    public AudioClip buttonPress1;

    private void Awake()
    {

    }

    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {

       AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);

        // Assign Audio Clip

        audioSource.clip = audioClip;
        // Assign Volume

        audioSource.volume = volume;
        // Play Sound
        audioSource.Play();

        // Get Length of Audio Clip
        float cliplength = audioSource.clip.length;

        // Destroy Clip Object
        Destroy(audioSource.gameObject, cliplength);


        // Assign to Prefab, specific sound
    }

    public void AudioList()
    {



    }
}
