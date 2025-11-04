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
    //[SerializeField] float _driveSpeed;
    [SerializeField] float _rotateSpeed, _brakeStrength, _maxDeflection, _nitroStrength;

    [SerializeField] CarModel _carModel;
    private WheelCollider _frontLeftWheel, _frontRightWheel;
    private WheelCollider _backLeftWheel, _backRightWheel;
    private WheelCollider[] _wheelColliders;
    
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
    //public float DriveSpeed => _driveSpeed * _speedMultiplier;
    //private float _speedMultiplier = 1;
    private float _startMass;
    private bool _isHandBroken;
    private Vector2 _axis;

    // настройка передач и оборотов
    private float _maxMotorTorque = 1500;
    private float _maxRPM = 7000f;
    private float _minRPM = 1000f;
    private float _currentMotorTorque;
    private float _engineRPM;
    private float _gearRatio = 1f;
    public float GearRatio
    {
        get => _gearRatio;
        set => _gearRatio = Mathf.Max(value, 0.1f);
    }
    public float EngineRPM => _engineRPM;

    private void Start()
    {
        _carModel = Instantiate(SaveManager.SelectedCar.Model, _rigidbody.transform).GetComponent<CarModel>();
        _carModel.Initialize(SaveManager.SelectedCar);
        _carModel.transform.Translate(0, -0.75f, 0);

        _frontLeftWheel = _carModel.WheelFL;
        _frontRightWheel = _carModel.WheelFR;
        _backLeftWheel = _carModel.WheelBL;
        _backRightWheel = _carModel.WheelBR;

        _wheelColliders = new WheelCollider[]{ _frontLeftWheel, _frontRightWheel, _backLeftWheel, _backRightWheel };

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
        if (_isHandBroken) 
            return;

        float targetRPM = axis.y > 0 ?
            _minRPM + axis.y * (_maxRPM - _minRPM) : 
            _minRPM;
        _engineRPM = Mathf.Lerp(_engineRPM, targetRPM, Time.deltaTime * 2f);
        _engineRPM = Mathf.Clamp(_engineRPM, _minRPM, _maxRPM);

        _currentMotorTorque = axis.y * _maxMotorTorque / _gearRatio;

        _frontLeftWheel.motorTorque = _currentMotorTorque * 5;
        _frontRightWheel.motorTorque = _currentMotorTorque * 5;
        _backLeftWheel.motorTorque = _currentMotorTorque * 5;
        _backRightWheel.motorTorque = _currentMotorTorque * 5;

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

        //_frontLeftWheel.brakeTorque = _brakeStrength * 50;
        //_frontRightWheel.brakeTorque = _brakeStrength * 50;

        //_frontLeftWheel.motorTorque = 0;
        //_frontRightWheel.motorTorque = 0;
    }
    /// <summary>
    /// Снять с ручного тормоза
    /// </summary>
    public void ReleaseHandBrake()
    {
        _isHandBroken = false;

        _backLeftWheel.brakeTorque = 0;
        _backRightWheel.brakeTorque = 0;
        //_frontLeftWheel.brakeTorque = 0;
        //_frontRightWheel.brakeTorque = 0;
    }
}
