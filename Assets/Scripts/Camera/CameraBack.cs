
using UnityEngine;

/// <summary>
/// Камера заднего вида
/// </summary>
public class CameraBack : CameraController
{
    [SerializeField] float _speed;
    public void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _car.transform.position + transform.rotation * _offset, _speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_angleX, _car.transform.eulerAngles.y + _angleY, 0), _speed * Time.deltaTime);
    }
}
