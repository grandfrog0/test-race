using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _music;
    private void Start()
    {
        AudioListener.pause = !SaveManager.SettingsData.IsSoundOn;
        _music.mute = !SaveManager.SettingsData.IsMusicOn;
    }
}
