using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle musicMuteToggle;
    [SerializeField] private Toggle sfxMuteToggle;

    private void Start()
    {
        if (musicSlider == null)
        {
            Debug.LogWarning("Settings musicSlider must be initialized in the inspector!");
        }

        if (sfxSlider == null)
        {
            Debug.LogWarning("Settings sfxSlider must be initialized in the inspector!");
        }

        if (musicMuteToggle == null)
        {
            Debug.LogWarning("Settings musicMuteToggle must be initialized in the inspector!");
        }

        if (sfxMuteToggle == null)
        {
            Debug.LogWarning("Settings sfxMuteToggle must be initialized in the inspector!");
        }
    }



    public void UpdateMusicVolume(float value)
    {
        AudioController.UpdateMusicVolume(value);
        musicMuteToggle.isOn = false;
    }

    public void UpdateSfxVolume(float value)
    {
        AudioController.UpdateSfxVolume(value);
        sfxMuteToggle.isOn = false;
    }

    public void ToggleMuteMusic(bool shouldMute)
    {
        if (shouldMute)
        {
            AudioController.MuteMusic();
        }

        else
        {
            UpdateMusicVolume(musicSlider.value);
        }
    }

    public void ToggleMuteSfx(bool shouldMute)
    {
        if (shouldMute)
        {
            AudioController.MuteSfx();
        }

        else
        {
            UpdateSfxVolume(sfxSlider.value);
        }
    }
}
