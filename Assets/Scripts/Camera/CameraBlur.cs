using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlur : MonoBehaviour
{
    [SerializeField] CarController _car;
    [SerializeField] float _multiplier = 1;
    private float _startFieldOfView;
    public void SetSpeed(float value)
    {
        if (_car.GearRatio  0) Camera.main.fieldOfView = _startFieldOfView + value / _car.GearRatio * _multiplier;
    }
    private void Start()
    {
        _startFieldOfView = Camera.main.fieldOfView;
    }
}
