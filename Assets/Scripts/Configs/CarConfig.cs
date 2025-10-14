using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "car_config", menuName = "Configs/Car")]
public class CarConfig : ScriptableObject
{
    public CarInfo Info;
    public GameObject Model;
}
