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
        Camera.main.fieldOfView = _startFieldOfView + value / Mathf.Max(_car.GearRatio, 1f) * _multiplier;
    }
    private void Start()
    {
        _startFieldOfView = Camera.main.fieldOfView;
    }
}
