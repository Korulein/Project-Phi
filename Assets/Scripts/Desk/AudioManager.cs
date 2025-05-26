using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioPrefab;
    public AudioSource bgmSource; // Dedicated source for BGM

    [Header("Sound FX Clips")]
    public AudioClip cameraFlash;
    public AudioClip buttonPress1;
    public AudioClip steelPickup;
    public AudioClip steelDrop;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip defaultBGM;
    [SerializeField][Range(0, 1)] private float bgmVolume = 1f;

    private void Awake()
    {
        // Singleton pattern implementation
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes

            // Set up BGM audio source
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            // Play default BGM if set
            if (defaultBGM != null) PlayBGM(defaultBGM);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Existing sound effect method
    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // ========== BGM Controls ==========
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    public void ChangeBGMVolume(float newVolume)
    {
        bgmVolume = Mathf.Clamp01(newVolume);
        bgmSource.volume = bgmVolume;
    }

    public void ChangeBGM(AudioClip newBGM)
    {
        if (bgmSource.clip != newBGM)
        {
            PlayBGM(newBGM);
        }
    }
}