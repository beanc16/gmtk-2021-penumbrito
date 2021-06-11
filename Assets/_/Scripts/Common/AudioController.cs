using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private const string GLOBAL_PREFS_KEY = "globalVolume";
    private const string MUSIC_PREFS_KEY = "MusicVolume";
    private const string SFX_PREFS_KEY = "SfxVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject sfxParent;
    [SerializeField] private GameObject musicParent;

    private readonly Dictionary<string, AudioSource> sfxSources = new Dictionary<string, AudioSource>();
    private readonly Dictionary<string, AudioSource> musicSources = new Dictionary<string, AudioSource>();

    public static AudioController Instance => AudioController.instance;
    private static AudioController instance;

    private float globalVolume;
    private float musicVolume;
    private float sfxVolume;

    private void Start()
    {
        AudioController.instance = this;

        this.globalVolume = PlayerPrefs.GetFloat(GLOBAL_PREFS_KEY);
        this.musicVolume = PlayerPrefs.GetFloat(MUSIC_PREFS_KEY);
        this.sfxVolume = PlayerPrefs.GetFloat(SFX_PREFS_KEY);

        var allSfx = this.sfxParent.GetComponentsInChildren<AudioSource>();
        foreach(var sfx in allSfx)
        {
            this.sfxSources.Add(sfx.name, sfx);
        }

        var allMusic = this.musicParent.GetComponentsInChildren<AudioSource>();
        foreach (var music in allMusic)
        {
            this.musicSources.Add(music.name, music);
        }
    }

    public static void PlaySfx(string sfxName)
    {
        instance.sfxSources[sfxName].Play();
    }

    public static void PlayMusic(string music)
    {
        instance.musicSources[music].Play();
    }

    public static void UpdateGlobalVolume(float newValue)
    {
        instance.globalVolume = newValue;

        PlayerPrefs.SetFloat(GLOBAL_PREFS_KEY, instance.globalVolume);

        instance.audioMixer.SetFloat("GlobalVolume", instance.globalVolume);
    }

    public static void UpdateMusicVolume(float newValue)
    {
        instance.musicVolume = newValue;
        PlayerPrefs.SetFloat(MUSIC_PREFS_KEY, instance.musicVolume);

        instance.audioMixer.SetFloat("MusicVolume", instance.musicVolume);
    }

    public static void UpdateSfxVolume(float newValue)
    {
        instance.sfxVolume = newValue;
        PlayerPrefs.SetFloat(SFX_PREFS_KEY, instance.sfxVolume);

        instance.audioMixer.SetFloat("SfxVolume", instance.sfxVolume);
    }
}