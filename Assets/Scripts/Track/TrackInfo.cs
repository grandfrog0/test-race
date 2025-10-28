using System;

/// <summary>
/// Сериализуемый объект, хранящий данные о трассе
/// </summary>
[Serializable]
public class TrackInfo
{
    public int Index;
    public int StarsCount;
    public float BestTime;
    public bool IsOpened;
}
