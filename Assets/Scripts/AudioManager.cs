using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance { get; set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Space]
    [Header("Clips")]
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _sfxClips;

    private Dictionary<string, AudioClip> _musicDictionary;
    private Dictionary<string, AudioClip> _sfxDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicDictionary = _musicClips.ToDictionary(clip => clip.name, clip => clip);
        _sfxDictionary = _sfxClips.ToDictionary(clip => clip.name, clip => clip);
    }

    public static void PlaySFX(string name)
    {
        if (Instance._sfxDictionary.TryGetValue(name, out var clip))
        {
            Instance._sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found.");
        }
    }

    public static void PlayMusic(string name, bool loop = true)
    {
        if (Instance._musicDictionary.TryGetValue(name, out var clip))
        {
            Instance._musicSource.clip = clip;
            Instance._musicSource.loop = loop;
            Instance._musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music '{name}' not found.");
        }
    }

    public static void StopSfx()
    {
        Instance._sfxSource.Stop();
    }

    public static void StopMusic()
    {
        Instance._musicSource.Stop();
    }

    public static void PlayClipAtPoint(string name, Vector3 position, float volume = 1f)
    {
        if (Instance._sfxDictionary.TryGetValue(name, out AudioClip audioClip))
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }
        else
        {
            Debug.LogWarning($"SFX {name} not found.");
        }
    }

    public static void SetMusicVolume(float volume) => Instance._musicSource.volume = volume;
    public static void SetSFXVolume(float volume) => Instance._sfxSource.volume = volume;
}
