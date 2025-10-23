using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт тахометра
/// </summary>
public class Tachometer : MonoBehaviour
{
    [SerializeField] Transform _arrow;
    [SerializeField] Vector2 _minMaxAngle;
    [SerializeField] float _maxSpeed;
    public void SetSpeed(float speed)
    {
        float value = speed / _maxSpeed;
        _arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(_minMaxAngle.x, _minMaxAngle.y, value));
    }
}
