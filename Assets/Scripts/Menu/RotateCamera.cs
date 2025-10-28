using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за вращение камеры вокруг автомобиля в главном меню
/// </summary>
public class RotateCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _speed = 1;
    [SerializeField] float _x, _y;
    [SerializeField] float _minDistance, _maxDistance;
    [SerializeField] float _scrollMultiplier = 1;
    [SerializeField] float _moveMultiplier = 1;
    [SerializeField] float _curDistance = 1;

    public void FixedUpdate()
    {
        _curDistance = Mathf.Clamp(_curDistance + Input.mouseScrollDelta.y * _scrollMultiplier, _minDistance, _maxDistance);
        _offset = _offset.normalized * Mathf.Lerp(_offset.magnitude, _curDistance, Time.fixedDeltaTime);

        if (Input.GetMouseButton(0))
            _y += Input.GetAxis("Mouse X") * _moveMultiplier;

        _y += _speed;
        _y %= 360;
        transform.forward = Quaternion.Euler(_x, _y, 0) * Vector3.forward;
        transform.position = _target.position - transform.rotation * _offset;
    }
}
