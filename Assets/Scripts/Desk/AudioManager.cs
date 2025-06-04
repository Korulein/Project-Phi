using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioPrefab;
    public AudioSource bgmSource;

    [Header("Sound FX Clips")]
    public AudioClip cameraFlash;
    public AudioClip buttonPress1;
    public AudioClip steelPickup;
    public AudioClip steelDrop;
    public AudioClip plasticPickup;
    public AudioClip plasticDrop;
    public AudioClip aerogelPickup;
    public AudioClip aerogelDrop;
    public AudioClip carbonFiberPickup;
    public AudioClip carbonFiberDrop;
    public AudioClip leadPickup;
    public AudioClip leadDrop;
    public AudioClip ceramicPickup;
    public AudioClip ceramicDrop;
    public AudioClip completeConstruction;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip defaultBGM;
    [SerializeField][Range(0, 1)] private float bgmVolume = 1f;

    private void Awake()
    {
        // Singleton 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up BGM audio source
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            if (defaultBGM != null) PlayBGM(defaultBGM);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
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