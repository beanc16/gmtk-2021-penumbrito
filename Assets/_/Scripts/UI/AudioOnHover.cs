using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string onHoverEnterAudioName;
    [SerializeField] private AudioType onHoverEnterAudioType = AudioType.Sfx;

    [SerializeField] private string onHoverExitAudioName;
    [SerializeField] private AudioType onHoverExitAudioType = AudioType.Sfx;

    private bool ShouldPlayHoverEnterAudio
    {
        get => (onHoverEnterAudioName.Length > 0);
    }

    private bool ShouldPlayHoverExitAudio
    {
        get => (onHoverExitAudioName.Length > 0);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ShouldPlayHoverEnterAudio)
        {
            if (onHoverEnterAudioType == AudioType.Sfx)
            {
                PlaySfx(onHoverEnterAudioName);
            }

            else if (onHoverEnterAudioType == AudioType.Music)
            {
                PlayMusic(onHoverEnterAudioName);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ShouldPlayHoverEnterAudio)
        {
            if (onHoverEnterAudioType == AudioType.Sfx)
            {
                PlaySfx(onHoverExitAudioName);
            }

            else if (onHoverEnterAudioType == AudioType.Music)
            {
                PlayMusic(onHoverExitAudioName);
            }
        }
    }



    private void PlaySfx(string str)
    {
        AudioController.PlaySfx(str);
    }
    
    private void PlayMusic(string str)
    {
        AudioController.PlayMusic(str);
    }
}



public enum AudioType
{
    Music,
    Sfx
}
