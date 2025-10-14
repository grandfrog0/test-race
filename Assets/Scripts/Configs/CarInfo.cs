using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CarInfo
{
    public GameObject Model => PrefabBuffer.instance.DefaultCarConfigs.Where(x => x.Info.Title == Title).First().Model;

    public string Title;
    public float Speed;
    public float Braking;
    public float Nitro;

    public bool IsOpened = false;
    public Color WheelColor;
    public Color SmokeColor;

    public CarInfo Clone() => new CarInfo()
    {
        Title = Title,
        Speed = Speed,
        Braking = Braking,
        Nitro = Nitro,
        IsOpened = IsOpened,
        WheelColor = WheelColor,
        SmokeColor = SmokeColor
    };
}
