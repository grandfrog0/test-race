using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Камера, привязанная к автомобилю
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] protected CarController _car;
    [SerializeField] protected Transform _parent;
    [SerializeField] protected Vector3 _offset;
    [SerializeField] protected float _angleX = 15;
    [SerializeField] protected float _angleY = 0;

    private void OnEnable()
    {
        transform.SetParent(_parent);
        transform.localRotation = Quaternion.Euler(_angleX, _angleY, 0);
        transform.position = _car.transform.position + transform.rotation * _offset;
    }
}
