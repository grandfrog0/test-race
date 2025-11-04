using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransmission : MonoBehaviour
{
    private bool _isAutomatic = true;

    private float[] _gearRatios = { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, 0.8f };
    private float _reverseGearRatio = -3.0f;
    private float _finalDriveRatio = 3.5f;

    private float[] _shiftUpRPM = { 3000, 3500, 4000, 4500, 5000 };
    private float[] _shiftDownRPM = { 1500, 2000, 2500, 3000, 3500 };
    private float _shiftDelay = 0.5f;

    private int _currentGear = 1;
    public int CurrentGear => _currentGear;
    private bool _isShifting = false;

    private CarController _controller;
    private CarModel _model;
    private List<WheelCollider> _wheelColliders;
    private float _lastShiftTime;

    void Start()
    {
        _controller = GetComponent<CarController>();
        _model = _controller.Model;
        _wheelColliders = new(){ _model.WheelFR, _model.WheelFL, _model.WheelBL, _model.WheelBR };
    }

    void Update()
    {
        // Переключение
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleTransmissionType();
        }

        if (_isAutomatic)
        {
            HandleAutomaticTransmission();
        }
        else
        {
            HandleManualTransmission();
        }

        ApplyGearRatio();
    }

    void HandleAutomaticTransmission()
    {
        if (_isShifting || Time.time - _lastShiftTime < _shiftDelay) 
            return;

        float currentRPM = GetEngineRPM();

        // Повышение передачи
        if (_currentGear > 0 && _currentGear < _gearRatios.Length &&
            currentRPM > _shiftUpRPM[_currentGear - 1])
        {
            ShiftUp();
        }
        // Понижение передачи
        else if (_currentGear > 1 && currentRPM < _shiftDownRPM[_currentGear - 2])
        {
            ShiftDown();
        }
    }

    void HandleManualTransmission()
    {
        if (_isShifting) return;

        // Переключение на повышенную передачу
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShiftUp();
        }
        // Переключение на пониженную передачу
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ShiftDown();
        }
    }

    public void ShiftUp()
    {
        if (_currentGear < _gearRatios.Length)
        {
            StartCoroutine(PerformShift(_currentGear + 1));
        }
    }

    public void ShiftDown()
    {
        if (_currentGear > 1)
        {
            StartCoroutine(PerformShift(_currentGear - 1));
        }
    }

    private IEnumerator PerformShift(int targetGear)
    {
        _isShifting = true;
        _lastShiftTime = Time.time;

        // Симуляция задержки переключения
        yield return new WaitForSeconds(0.1f);

        _currentGear = targetGear;
        _isShifting = false;

        Debug.Log($"Переключение на передачу: {targetGear}");
    }

    void ApplyGearRatio()
    {
        if (_controller == null) return;

        float gearRatio = 0f;

        if (_currentGear == -1) // Задняя передача
        {
            gearRatio = _reverseGearRatio * _finalDriveRatio;
        }
        else if (_currentGear > 0 && _currentGear <= _gearRatios.Length) // Передние передачи
        {
            gearRatio = _gearRatios[_currentGear - 1] * _finalDriveRatio;
        }

        // Применяем передаточное число к двигателю
        _controller.GearRatio = gearRatio;
    }

    public void ToggleTransmissionType()
    {
        _isAutomatic = !_isAutomatic;

        Debug.Log($"Режим: {(_isAutomatic ? "Автомат" : "Ручной")}");
    }

    float GetEngineRPM()
    {
        // Получаем RPM от двигателя автомобиля
        if (_controller != null)
        {
            return _controller.EngineRPM;
        }

        // Запасной вариант расчета RPM
        float wheelRPM = 0f;
        foreach (var wheel in _wheelColliders)
        {
            wheelRPM += Mathf.Abs(wheel.rpm);
        }
        wheelRPM /= _wheelColliders.Count;

        float currentGearRatio = _currentGear == -1 ? _reverseGearRatio :
                                (_currentGear > 0 ? _gearRatios[_currentGear - 1] : 1f);

        return wheelRPM * currentGearRatio * _finalDriveRatio;
    }

    float GetCarSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // км/ч
    }
}