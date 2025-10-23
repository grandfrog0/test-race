using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за покраску частей автомобиля
/// </summary>
public class CustomizeVehicleManager : MonoBehaviour
{
    private ColorizeMode _colorizeMode = ColorizeMode.Wheels;
    public void SetColorizeMode(int colorizeMode)
        => _colorizeMode = (ColorizeMode)colorizeMode;
    public void SetColor(int color)
    {
        if (_colorizeMode == ColorizeMode.Wheels)
            SetWheelsColor(PrefabBuffer.instance.CarColors[color]);
        else if (_colorizeMode == ColorizeMode.Smoke)
            SetSmokeColor(PrefabBuffer.instance.CarColors[color]);
    }
    public void SetRandomColor()
    {
        if (_colorizeMode == ColorizeMode.Wheels)
            SetWheelsColor(Random.ColorHSV());
        else if (_colorizeMode == ColorizeMode.Smoke)
            SetSmokeColor(Random.ColorHSV());
    }

    public void SetWheelsColor(Color color)
    {
        if (SaveManager.Data.SelectedCar != null) SaveManager.Data.SelectedCar.WheelColor = color;
        VehiclesManager.CurModel?.SetWheelsColor(color);
    }
    public void SetSmokeColor(Color color)
    {
        if (SaveManager.Data.SelectedCar != null) SaveManager.Data.SelectedCar.SmokeColor = color;
        VehiclesManager.CurModel?.SetSmokeColor(color);
    }
}
