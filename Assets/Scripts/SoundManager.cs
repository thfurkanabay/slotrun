using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton instance

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // For background music
    [SerializeField] private AudioSource sfxSource;   // For sound effects

    [Header("SFX Clips")]
    public List<SoundClip> soundClips; // Custom class list for organizing sounds

    private Dictionary<SoundEffect, AudioClip> sfxDictionary;

    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;


    public enum SoundEffect
    {
        ButtonClick,
        CollectiblePickup,
        MenuOpen,
        GameOver,
        Spin,
        ObjectAppear,
        ObstacleAnim,
        Win,
        RewardCoinCollect,
        RewardGemCollect,
        RewardXPCollect

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateVolumes();
        sfxDictionary = new Dictionary<SoundEffect, AudioClip>();
        foreach (SoundClip soundClip in soundClips)
        {
            sfxDictionary.Add(soundClip.soundEffect, soundClip.clip);
        }

    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }


    public void PlaySFX(SoundEffect effect)
    {
        if (sfxDictionary.TryGetValue(effect, out AudioClip clip))
        {
            Debug.Log($"Playing sound effect: {effect}");
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{effect}' not found in SoundManager.");
        }
    }
    public void PlaySFXByName(string effectName)
    {
        if (System.Enum.TryParse(effectName, out SoundEffect effect))
        {
            PlaySFX(effect);
        }
    }

    public void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    [System.Serializable]
    public class SoundClip
    {
        public SoundEffect soundEffect; // Enum for identifying sounds
        public AudioClip clip;          // Corresponding AudioClip
    }

    public IEnumerator SlowAndStopMusic()
    {
        float startPitch = musicSource.pitch;
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;
        float fadeDuration = 2f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            musicSource.pitch = Mathf.Lerp(startPitch, 0.1f, progress);
            musicSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        musicSource.Stop();
        musicSource.pitch = 1f;
    }
    public void ResetMusic()
    {
        if (musicSource != null)
        {
            musicSource.pitch = 1f;
            musicSource.volume = musicVolume;
        }
    }
}