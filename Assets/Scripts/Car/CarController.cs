using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������ ���������� �����������
/// </summary>
public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _driveSpeed, _rotateSpeed, _brakeStrength, _maxDeflection, _nitroStrength;

    [SerializeField] CarModel _carModel;
    private WheelCollider _frontLeftWheel, _frontRightWheel;
    private WheelCollider _backLeftWheel, _backRightWheel;
    
    /// <summary>
    /// �������� ������� � ������ ����������
    /// </summary>
    public CarModel Model => _carModel;
    /// <summary>
    /// ��������� �� ���������� � ��������� ������
    /// </summary>
    public bool IsDrifting => Velocity.magnitude > 1 && DriftAngle < 90 && DriftAngle > _maxDeflection;
    /// <summary>
    /// �������� ����������
    /// </summary>
    public Vector2 Velocity => new(_rigidbody.velocity.x, _rigidbody.velocity.z);
    /// <summary>
    /// ���� ������ ����������
    /// </summary>
    public float DriftAngle => Vector3.Angle(_rigidbody.velocity.normalized, transform.forward);
    /// <summary>
    /// ���������� ����� �����
    /// </summary>
    public float Nitro { get; set; } = 100;

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
    
    /// <summary>
    /// ������������ ����� ��� ���������
    /// </summary>
    public void UseNitro()
    {
        if (Nitro <= 0) 
            return;

        Nitro -= 10 * Time.deltaTime;
        _rigidbody.AddForce(transform.forward * _nitroStrength, ForceMode.Acceleration);
    }
    /// <summary>
    /// �������� �������� � ���� ��������
    /// </summary>
    /// <param name="axis"></param>
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
    /// <summary>
    /// ���������
    /// </summary>
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
    /// <summary>
    /// ��������� ������
    /// </summary>
    public void ReleaseTorque()
    {
        _frontLeftWheel.brakeTorque = 0;
        _frontRightWheel.brakeTorque = 0;
        _backLeftWheel.brakeTorque = 0;
        _backRightWheel.brakeTorque = 0;
    }
}
