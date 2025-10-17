using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _driveSpeed, _rotateSpeed, _brakeStrength, _maxDeflection;

    [SerializeField] CarModel _carModel;
    private WheelCollider _frontLeftWheel, _frontRightWheel;
    private WheelCollider _backLeftWheel, _backRightWheel;

    public bool IsDrifting => Velocity.magnitude > 1 && DriftAngle < 90 && DriftAngle > _maxDeflection;
    public Vector2 Velocity => new(_rigidbody.velocity.x, _rigidbody.velocity.z);
    public float DriftAngle => Vector3.Angle(_rigidbody.velocity.normalized, transform.forward);

    private void Start()
    {
        _carModel = Instantiate(SaveManager.Data.SelectedCar.Model, _rigidbody.transform).GetComponent<CarModel>();
        _carModel.Initialize(SaveManager.Data.SelectedCar);
        _carModel.transform.Translate(0, -0.75f, 0);

        _frontLeftWheel = _carModel.WheelFL;
        _frontRightWheel = _carModel.WheelFR;
        _backLeftWheel = _carModel.WheelBL;
        _backRightWheel = _carModel.WheelBR;
    }

    public void SetAxis(Vector2 axis)
    {
        float speed = axis.y * _driveSpeed;

        _frontLeftWheel.motorTorque = speed;
        _frontRightWheel.motorTorque = speed;
        _backLeftWheel.motorTorque = speed;
        _backRightWheel.motorTorque = speed;

        float angle = axis.x * _rotateSpeed;
        _frontLeftWheel.steerAngle = angle;
        _frontRightWheel.steerAngle = angle;
    }
    public void BrakeTorque()
    {
        _frontLeftWheel.brakeTorque = _brakeStrength;
        _frontRightWheel.brakeTorque = _brakeStrength;
        _backLeftWheel.brakeTorque = _brakeStrength;
        _backRightWheel.brakeTorque = _brakeStrength;

        _frontLeftWheel.motorTorque = 0;
        _frontRightWheel.motorTorque = 0;
        _backLeftWheel.motorTorque = 0;
        _backRightWheel.motorTorque = 0;
    }
    public void ReleaseTorque()
    {
        _frontLeftWheel.brakeTorque = 0;
        _frontRightWheel.brakeTorque = 0;
        _backLeftWheel.brakeTorque = 0;
        _backRightWheel.brakeTorque = 0;
    }
}
