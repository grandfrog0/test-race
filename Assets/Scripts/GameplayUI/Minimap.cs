using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Transform _rotateTransform;
    [SerializeField] Transform _target;
    [SerializeField] float _positionMultiplier = -1.8f;

    public void FixedUpdate()
    {
        _rotateTransform.localRotation = Quaternion.Euler(0, 0, _target.eulerAngles.y);
        _rect.anchoredPosition = new Vector3(_target.position.x / _positionMultiplier, _target.position.z / _positionMultiplier, 0);
    }
}
