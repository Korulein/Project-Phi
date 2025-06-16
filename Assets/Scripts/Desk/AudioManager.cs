using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioPrefab;
    public AudioSource bgmSource;

    [Header("Component SFX Clips")]
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

    [Header("UI SFX Clips")]
    public AudioClip buttonPress1;
    public AudioClip completeConstruction;
    public AudioClip orderComponent;
    public AudioClip errorSound;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip defaultBGM;
    [SerializeField][Range(0, 1)] private float bgmVolume = 1f;

    [Header("Button Press Cooldown")]
    public bool canPress = true;
    [SerializeField] private float buttonCooldown = 0.4f;
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

    #region Play SFX
    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (!canPress)
            return;

        StartCoroutine(OrderComponentCooldown());

        AudioSource audioSource = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    #endregion

    #region BGM
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
    #endregion

    #region Helper method
    public IEnumerator OrderComponentCooldown()
    {
        canPress = false;
        yield return new WaitForSeconds(buttonCooldown);
        canPress = true;
    }
    #endregion
}