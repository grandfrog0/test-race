using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarTransmission : MonoBehaviour
{
    private bool _isAutomatic = true;
    public bool IsAutomatic => _isAutomatic;

    private float[] _gearRatios = { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, 0.8f };
    private float _reverseGearRatio = -3.0f;
    private float _finalDriveRatio = 3.5f;

    private float[] _shiftUpRPM = { 3000, 3500, 4000, 4500, 5000, 5500 };
    private float[] _shiftDownRPM = { 1500, 2000, 2500, 3000, 3500, 4000 };
    private float _reverseShiftUp = 3500;
    private float _reverseShiftDown = 2000;
    private float _shiftDelay = 0.5f;

    private int _currentGear = 1;
    public int CurrentGear { get => _currentGear; set => _currentGear = value; }
    public bool IsClutchPressed { get; set; } = false;

    private CarController _controller;
    private CarModel _model;
    private List<WheelCollider> _wheelColliders;
    private float _lastShiftTime;
    private Coroutine _coroutine;

    void Start()
    {
        _controller = GetComponent<CarController>();
        _model = _controller.Model;
        _wheelColliders = new(){ _model.WheelFR, _model.WheelFL, _model.WheelBL, _model.WheelBR };

        _coroutine = StartCoroutine(HandleAutomaticTransmission());
    }

    private IEnumerator HandleAutomaticTransmission()
    {
        while (true)
        {
            if (Time.time - _lastShiftTime < _shiftDelay)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            // Повышение передачи
            if (_currentGear > 0 && _currentGear < _gearRatios.Length &&
                _controller.EngineRPM >= _shiftUpRPM[_currentGear - 1] - 10)
            {
                ShiftUp();
            }
            // Понижение передачи
            else if (_currentGear > 1 && _controller.EngineRPM < _shiftDownRPM[_currentGear - 2])
            {
                ShiftDown();
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShiftTo(bool up)
    {
        if (_isAutomatic || !IsClutchPressed)
            return;

        if (up)
        {
            ShiftUp();
        }
        else
        {
            ShiftDown();
        }
    }

    public void ShiftUp()
    {
        if (_currentGear < _gearRatios.Length)
        {
            PerformShift(_currentGear + 1);
        }
    }

    public void ShiftDown()
    {
        if (_currentGear > -1)
        {
            PerformShift(_currentGear - 1);
        }
    }

    private void PerformShift(int targetGear)
    {
        _lastShiftTime = Time.time;

        _currentGear = targetGear;

        ApplyGearRatio();

        Debug.Log($"Переключение на передачу: {targetGear}");
    }

    private void ApplyGearRatio()
    {
        if (_controller == null) return;

        float gearRatio = 0f;
        float minRPM = 0f;
        float maxRPM = 0f;

        if (_currentGear == -1)
        {
            gearRatio = _reverseGearRatio * _finalDriveRatio;
            minRPM = _reverseShiftDown;
            maxRPM = _reverseShiftUp;
        }
        else if (_currentGear > 0 && _currentGear <= _gearRatios.Length)
        {
            gearRatio = _gearRatios[_currentGear - 1] * _finalDriveRatio;
            minRPM = _shiftDownRPM[_currentGear - 1];
            maxRPM = _shiftUpRPM[_currentGear - 1];
        }

        _controller.GearRatio = gearRatio;
        _controller.MinRPM = minRPM;
        _controller.MaxRPM = maxRPM;
    }

    public void ToggleTransmission()
    {
        _isAutomatic = !_isAutomatic;

        if (!_coroutine.IsUnityNull())
        {
            StopCoroutine(_coroutine);
        }
        if (_isAutomatic)
        {
            _coroutine = StartCoroutine(HandleAutomaticTransmission());
        }

        Debug.Log("Режим: " + (_isAutomatic ? "Автомат" : "Ручной"));
    }
}