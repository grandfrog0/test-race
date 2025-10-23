using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Объект для сериализации данных о текущем прогрессе и настройках
/// </summary>
[Serializable]
public class GameData
{
    public int MoneyCount = 999999;
    public int SelectedCarIndex;
    public List<CarInfo> Cars = new();
    public List<TrackInfo> Tracks = new();
    public SettingsData Settings = new();
    public CarInfo SelectedCar => Cars[SelectedCarIndex];
}
