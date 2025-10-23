using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Конфигурация автомобиля
/// </summary>
[CreateAssetMenu(fileName = "car_config", menuName = "Configs/Car")]
public class CarConfig : ScriptableObject
{
    public CarInfo Info;
    public GameObject Model;
}
