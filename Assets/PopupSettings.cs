using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettings : Popup
{
    public GameObject musicOffIcon;
    public GameObject sfxOffIcon;


    void Update()
    {
        IsVolumesMuted();
    }
    public void IsVolumesMuted()
    {
        if (SoundManager.Instance.musicMuted)
        {
            musicOffIcon.SetActive(true);
        }
        else
        {
            musicOffIcon.SetActive(false);
        }
        if (SoundManager.Instance.sfxMuted)
        {
            sfxOffIcon.SetActive(true);
        }
        else
        {
            sfxOffIcon.SetActive(false);
        }
    }
}
