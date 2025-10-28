using UnityEngine;

/// <summary>
/// Конфигурация автомобиля
/// </summary>
[CreateAssetMenu(fileName = "car_config", menuName = "Configs/Car")]
public class CarConfig : ScriptableObject
{
    public string ConfigPath;
    private CarInfo _info;
    public CarInfo Info => _info;
    public GameObject Model;
    public void OnEnable()
    {
        string json = Resources.Load<TextAsset>(ConfigPath).text;
        _info = JsonUtility.FromJson<CarInfo>(json);
    }
}
