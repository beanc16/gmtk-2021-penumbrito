using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    private static bool isInitialized = false;
    public static bool IsInitialized
    {
        get => isInitialized;
    }

    public static void TryInitializeAudioVolumes()
    {
        TryInitializeMusicVolume();
        TryInitializeSfxVolume();

        isInitialized = true;
    }

    private static void TryInitializeMusicVolume()
    {
        float musicVolume = Settings.GetSavedMusicVolume();
        bool musicIsMuted = Settings.SavedMusicIsMuted();

        if (PlayerPrefs.HasKey("musicIsMuted"))
        {
            if (musicIsMuted)
            {
                AudioController.UpdateMusicVolume(0);
            }
        }

        else if (musicVolume != null)
        {
            AudioController.UpdateMusicVolume(musicVolume);
        }
    }

    private static void TryInitializeSfxVolume()
    {
        float sfxVolume = Settings.GetSavedSfxVolume();
        bool sfxIsMuted = Settings.SavedSfxIsMuted();

        if (PlayerPrefs.HasKey("sfxIsMuted"))
        {
            if (sfxIsMuted)
            {
                AudioController.UpdateSfxVolume(0);
            }
        }

        else if (sfxVolume != null)
        {
            AudioController.UpdateSfxVolume(sfxVolume);
        }
    }
}
