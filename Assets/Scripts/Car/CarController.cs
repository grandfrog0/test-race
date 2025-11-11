using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Скрипт управления автомобилем
/// </summary>
public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] GroundChecker _groundChecker;
    [SerializeField] float _rotateSpeed, _brakeStrength, _maxDeflection, _nitroStrength;
    [SerializeField] float _speedMultiplier = 2f;

    [SerializeField] CarModel _carModel;
    private WheelCollider _frontLeftWheel, _frontRightWheel;
    private WheelCollider _backLeftWheel, _backRightWheel;
    
    /// <summary>
    /// Свойство доступа к модели автомобиля
    /// </summary>
    public CarModel Model => _carModel;
    /// <summary>
    /// Находится ли автомобиль в состоянии заноса
    /// </summary>
    public bool IsDrifting => _groundChecker.IsOnGround && Velocity.magnitude > 1 && DriftAngle < 90 && DriftAngle > _maxDeflection;
    /// <summary>
    /// Скорость автомобиля
    /// </summary>
    public Vector2 Velocity => new(_rigidbody.velocity.x, _rigidbody.velocity.z);
    /// <summary>
    /// Угол заноса автомобиля
    /// </summary>
    public float DriftAngle => Vector3.Angle(_rigidbody.velocity.normalized, transform.forward);
    /// <summary>
    /// Оставшийся запас нитро
    /// </summary>
    public float Nitro { get; set; } = 100;
    private float _startMass;
    private bool _isHandBroken;
    private bool _isTorqueReleased = true;

    // Настройка передач и оборотов
    private CarTransmission _transmission;
    private float _maxMotorTorque = 1500;
    private float _maxRPM = 7000f;
    private float _minRPM = 500f;
    private float _currentMotorTorque;
    private float _engineRPM;
    private float _gearRatio = 1f;
    private Vector2 _axis;
    public float GearRatio
    {
        get => _gearRatio;
        set => _gearRatio = value;
    }
    public float EngineRPM => _engineRPM;
    public bool IsStalled { get; set; } // заглох ли автомобиль
    public float MaxRPM { get => _maxRPM; set => _maxRPM = value; }
    public float MinRPM { get => _minRPM; set => _minRPM = value; }

    private void Start()
    {
        _transmission = GetComponent<CarTransmission>();

        _carModel = Instantiate(SaveManager.SelectedCar.Model, _rigidbody.transform).GetComponent<CarModel>();
        _carModel.Initialize(SaveManager.SelectedCar);
        _carModel.transform.Translate(0, -0.75f, 0);

        _frontLeftWheel = _carModel.WheelFL;
        _frontRightWheel = _carModel.WheelFR;
        _backLeftWheel = _carModel.WheelBL;
        _backRightWheel = _carModel.WheelBR;

        _startMass = _rigidbody.mass;
    }
    
    /// <summary>
    /// Использовать нитро для ускорения
    /// </summary>
    public void UseNitro()
    {
        if (Nitro <= 0)
        {
            StopNitro();
            return;
        }

        Nitro -= 5  * Time.deltaTime;
        _rigidbody.mass = _startMass / 5;
    }
    /// <summary>
    /// Выключить нитро
    /// </summary>
    public void StopNitro()
    {
        _rigidbody.mass = _startMass;
    }
    /// <summary>
    /// Передать скорость и угол поворота
    /// </summary>
    /// <param name="axis"></param>
    public void SetAxis(Vector2 axis)
    {
        if (_isTorqueReleased && !_transmission.IsAutomatic)
        {
            if (!IsStalled)
            {
                if ((_engineRPM <= MinRPM && _currentMotorTorque > 0) || (DriftAngle > 90 || _rigidbody.velocity.magnitude > 1))
                {
                    IsStalled = true;
                }
            }
            else
            {
                if (_transmission.IsClutchPressed && axis.y > 0)
                {
                    IsStalled = false;
                }
            }
        }

        float targetRPM;
        if (_transmission.IsAutomatic)
        {
            IsStalled = false;
            targetRPM = axis.y > 0 ? _minRPM + axis.y * (_maxRPM - _minRPM) : 0;
        }
        else
        {
           targetRPM = axis.y > 0 ? _minRPM + axis.y * (_maxRPM - _minRPM) : 0;
        }

        _engineRPM = Mathf.Lerp(_engineRPM, targetRPM, Time.deltaTime * 0.5f);

        _currentMotorTorque =
            _gearRatio == 0 ?
            Mathf.Lerp(_currentMotorTorque, 0, Time.deltaTime) :
            axis.y * _maxMotorTorque / _gearRatio;

        if (!_transmission.IsClutchPressed && !_isHandBroken && !IsStalled && _transmission.CurrentGear != 0)
        {
            _frontLeftWheel.motorTorque = _currentMotorTorque * _speedMultiplier;
            _frontRightWheel.motorTorque = _currentMotorTorque * _speedMultiplier;
            _backLeftWheel.motorTorque = _currentMotorTorque * _speedMultiplier;
            _backRightWheel.motorTorque = _currentMotorTorque * _speedMultiplier;
        }
        else
        {
            _frontLeftWheel.motorTorque = Mathf.Lerp(_frontLeftWheel.motorTorque, 0, Time.deltaTime);
            _frontRightWheel.motorTorque = Mathf.Lerp(_frontRightWheel.motorTorque, 0, Time.deltaTime);
            _backLeftWheel.motorTorque = Mathf.Lerp(_backLeftWheel.motorTorque, 0, Time.deltaTime);
            _backRightWheel.motorTorque = Mathf.Lerp(_backRightWheel.motorTorque, 0, Time.deltaTime);
        }

            float angle = axis.x * _rotateSpeed;
        _frontLeftWheel.steerAngle = angle;
        _frontRightWheel.steerAngle = angle;

        _carModel.SetReverseLightsActive(DriftAngle > 90 && _rigidbody.velocity.magnitude > 1);
        _carModel.SetStopLightsActive(axis.y < 0 && !(DriftAngle > 90 && _rigidbody.velocity.magnitude > 1));

        _axis = axis;
    }
    /// <summary>
    /// Тормозить
    /// </summary>
    public void BrakeTorque()
    {
        if (_axis.y < 0 && !(DriftAngle > 90 && _rigidbody.velocity.magnitude > 0))
        {
            _frontLeftWheel.brakeTorque = _brakeStrength;
            _frontRightWheel.brakeTorque = _brakeStrength;
            _backLeftWheel.brakeTorque = _brakeStrength;
            _backRightWheel.brakeTorque = _brakeStrength;

            _frontLeftWheel.motorTorque = 0;
            _frontRightWheel.motorTorque = 0;
            _backLeftWheel.motorTorque = 0;
            _backRightWheel.motorTorque = 0;

            _isTorqueReleased = false;
        }
        else
        {
            ReleaseTorque();
        }
    }
    /// <summary>
    /// Отпустить тормоз
    /// </summary>
    public void ReleaseTorque()
    {
        _frontLeftWheel.brakeTorque = 0;
        _frontRightWheel.brakeTorque = 0;
        _backLeftWheel.brakeTorque = 0;
        _backRightWheel.brakeTorque = 0;
        
        _isTorqueReleased = true;
    }
    /// <summary>
    /// Ручной тормоз
    /// </summary>
    public void HandBrake()
    {
        _isHandBroken = true;

        _backLeftWheel.brakeTorque = _brakeStrength * 50;
        _backRightWheel.brakeTorque = _brakeStrength * 50;

        _backLeftWheel.motorTorque = 0;
        _backRightWheel.motorTorque = 0;
    }
    /// <summary>
    /// Снять с ручного тормоза
    /// </summary>
    public void ReleaseHandBrake()
    {
        _isHandBroken = false;

        _backLeftWheel.brakeTorque = 0;
        _backRightWheel.brakeTorque = 0;
    }
}
