using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Toggle _soundToggle, _musicToggle, _keyboardToggle;
    [SerializeField] Slider _sensitivitySlider;

    public void Start()
        => UpdateSettingsWindow();
    public void UpdateSettingsWindow()
    {
        QualitySettings.SetQualityLevel(SaveManager.Data.Settings.QualityLevel);

        _soundToggle.isOn = SaveManager.Data.Settings.IsSoundOn;
        _musicToggle.isOn = SaveManager.Data.Settings.IsMusicOn;
        _keyboardToggle.isOn = SaveManager.Data.Settings.IsKeyboardOn;
        _sensitivitySlider.value = SaveManager.Data.Settings.Sensitivity;
    }
    public void SetQualityLevel(int value)
    {
        SaveManager.Data.Settings.QualityLevel = value;
        QualitySettings.SetQualityLevel(value);
    }
    public void SetSoundOn(bool value)
        => SaveManager.Data.Settings.IsSoundOn = value;
    public void SetMusicOn(bool value)
        => SaveManager.Data.Settings.IsMusicOn = value;
    public void SetKeyboardOn(bool value)
        => SaveManager.Data.Settings.IsKeyboardOn = value;
    public void SetSensitivityOn(float value)
        => SaveManager.Data.Settings.Sensitivity = value;
    public void ResetSettings()
    {
        SaveManager.Data.Settings.Reset();
        UpdateSettingsWindow();
    }
}
