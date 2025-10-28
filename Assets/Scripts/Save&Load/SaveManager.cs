using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Отвечает за загрузку, хранение и выгрузку прогресса
/// </summary>
public class SaveManager : MonoBehaviour
{
    private static string _folderName = "TestRace";
    private static string _carsFile = "cars.json";
    private static string _tracksFile = "tracks.json";
    private static string _settingsFile = "settings.json";
    private static string _playerFile = "player.json";
    public static string FolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _folderName);
    public static string FilePath(string fileName) => Path.Combine(FolderPath, fileName);

    public static List<CarInfo> Cars { get => SerializableCars.Values; set => SerializableCars.Values = value; }
    public static List<TrackInfo> Tracks { get => SerializableTracks.Values; set => SerializableTracks.Values = value; }
    private static SerializableList<CarInfo> SerializableCars { get; set; }
    private static SerializableList<TrackInfo> SerializableTracks { get; set; }
    public static SettingsData SettingsData { get; set; }
    public static PlayerData PlayerData { get; set; }
    public static CarInfo SelectedCar => Cars[PlayerData.SelectedCarIndex];

    public static void SaveData()
    {
        try
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            string json = JsonUtility.ToJson(SerializableCars);
            File.WriteAllText(FilePath(_carsFile), json);

            json = JsonUtility.ToJson(SerializableTracks);
            File.WriteAllText(FilePath(_tracksFile), json);

            json = JsonUtility.ToJson(SettingsData);
            File.WriteAllText(FilePath(_settingsFile), json);

            json = JsonUtility.ToJson(PlayerData);
            File.WriteAllText(FilePath(_playerFile), json);
        }
        catch (Exception e)
        {
            Debug.Log("Error on save occured: " + e);
        }
    }
    public static void LoadData()
    {
        try
        {
            string json = File.ReadAllText(FilePath(_carsFile));
            SerializableCars = JsonUtility.FromJson<SerializableList<CarInfo>>(json) ?? new();

            json = File.ReadAllText(FilePath(_tracksFile));
            SerializableTracks = JsonUtility.FromJson<SerializableList<TrackInfo>>(json) ?? new();

            json = File.ReadAllText(FilePath(_settingsFile));
            SettingsData = JsonUtility.FromJson<SettingsData>(json) ?? new();

            json = File.ReadAllText(FilePath(_playerFile));
            PlayerData = JsonUtility.FromJson<PlayerData>(json) ?? new();
        }
        catch (Exception e)
        {
            Debug.Log("Error on load occured: " + e);
            ClearDataWithoutNotify();
        }
    }
    private static void ClearDataWithoutNotify()
    {
        SerializableCars = new();
        SerializableTracks = new();
        Debug.Log(Resources.Load<TextAsset>("Configs/settings"));
        SettingsData = JsonUtility.FromJson<SettingsData>(Resources.Load<TextAsset>("Configs/settings").text);
        PlayerData = JsonUtility.FromJson<PlayerData>(Resources.Load<TextAsset>("Configs/player_data").text);
    }
    public void ClearData()
    {
        ClearDataWithoutNotify();

        SaveData();
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        if (SerializableCars == null)
            LoadData();
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
}
