using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioClick : MonoBehaviour
{
    [SerializeField] private string onClickFileName;
    [SerializeField] private AudioType onClickAudioType = AudioType.Sfx;

    private bool ShouldPlayHoverEnterAudio
    {
        get => (onClickFileName.Length > 0);
    }



    public void OnClick()
    {
        if (ShouldPlayHoverEnterAudio)
        {
            if (onClickAudioType == AudioType.Sfx)
            {
                PlaySfx(onClickFileName);
            }

            else if (onClickAudioType == AudioType.Music)
            {
                PlayMusic(onClickFileName);
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
