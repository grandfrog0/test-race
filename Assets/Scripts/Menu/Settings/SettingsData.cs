using System;

[Serializable]
public class SettingsData
{
    public int QualityLevel = 1;
    public bool IsSoundOn = true;
    public bool IsMusicOn = true;
    public bool IsKeyboardOn = true;
    public float Sensitivity = 0.5f;

    public void Reset()
    {
        QualityLevel = 1;
        IsSoundOn = true;
        IsMusicOn = true;
        IsKeyboardOn = true;
        Sensitivity = 0.5f;
    }
}
