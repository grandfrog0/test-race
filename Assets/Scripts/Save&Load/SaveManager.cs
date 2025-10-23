using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Отвечает за загрузку, хранение и выгрузку прогресса
/// </summary>
public class SaveManager : MonoBehaviour
{
    private static string _folderName = "TestRace";
    private static string _fileName = "save.json";
    public static string FolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _folderName);
    public static string FilePath => Path.Combine(FolderPath, _fileName);
    public static GameData Data { get; set; }

    public static void SaveData()
    {
        string json = JsonUtility.ToJson(Data);
        try
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.Log("Error on save occured: " + e);
        }
    }
    public static void LoadData()
    {
        Debug.Log(FilePath);
        try
        {
            string json = File.ReadAllText(FilePath);
            Data = JsonUtility.FromJson<GameData>(json) ?? new();
        }
        catch (Exception e)
        {
            Debug.Log("Error on load occured: " + e);
            Data = new();
        }
        Debug.Log(Data);
    }
    public void ClearData()
    {
        Data = new();
        SaveData();
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        if (Data == null)
            LoadData();
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
}
