using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _speed = 1;
    [SerializeField] float _x, _y;
    public void FixedUpdate()
    {
        _y += _speed;
        _y %= 360;
        transform.forward = Quaternion.Euler(_x, _y, 0) * Vector3.forward;
        transform.position = _target.position - transform.rotation * _offset;
    }
}
