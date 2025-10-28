using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Отвечает за отображение и редактирования настроек
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> _onSoundOnChanged, _onMusicOnChanged;

    [SerializeField] Toggle _soundToggle, _musicToggle;
    [SerializeField] Toggle _keyboardControlToggle, _mouseControlToggle;
    [SerializeField] Slider _sensitivitySlider;

    public void Start()
        => UpdateSettingsWindow();
    public void UpdateSettingsWindow()
    {
        QualitySettings.SetQualityLevel(SaveManager.SettingsData.QualityLevel);

        _soundToggle.isOn = SaveManager.SettingsData.IsSoundOn;
        _musicToggle.isOn = SaveManager.SettingsData.IsMusicOn;
        _keyboardControlToggle.isOn = SaveManager.SettingsData.IsKeyboardOn;
        _mouseControlToggle.isOn = !_keyboardControlToggle.isOn;
        _sensitivitySlider.value = SaveManager.SettingsData.Sensitivity;
    }
    public void SetQualityLevel(int value)
    {
        SaveManager.SettingsData.QualityLevel = value;
        QualitySettings.SetQualityLevel(value);
    }
    public void SetSoundOn(bool value)
    {
        SaveManager.SettingsData.IsSoundOn = value;
        _onSoundOnChanged.Invoke(!value);
    }
    public void SetMusicOn(bool value)
    {
        SaveManager.SettingsData.IsMusicOn = value;
        _onMusicOnChanged.Invoke(!value);
    }
    public void SetKeyboardOn(bool value) 
        => SaveManager.SettingsData.IsKeyboardOn = value;
    public void SetMouseOn(bool value) 
        => SetKeyboardOn(!value);
    public void SetSensitivityOn(float value)
        => SaveManager.SettingsData.Sensitivity = value;
    public void ResetSettings()
    {
        SaveManager.SettingsData.Reset();
        UpdateSettingsWindow();
    }
}
