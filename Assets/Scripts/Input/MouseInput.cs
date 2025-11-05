
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за обработку нажатий на клавиатуре
/// </summary>
public class MouseInput : MonoBehaviour
{
    [SerializeField] CarController _carController;
    [SerializeField] PauseButton _pauseButton;
    private CarTransmission _carTransmission;
    private float _lastMouse0PressedTime;

    private float _mouseDelta => Input.GetAxis("Mouse X");

    public void Update()
    {
        Vector2 axis = new Vector2(Mathf.Clamp(_mouseDelta, -1, 1), ToInt(Input.GetMouseButton(0)));
        _carController.SetAxis(axis);
        if (axis.y < 0)
        {
            if (Input.GetMouseButtonDown(ToInt(Input.GetMouseButton(1))))
                _carController.BrakeTorque();
            if (Input.GetMouseButtonUp(ToInt(Input.GetMouseButton(1))))
                _carController.ReleaseTorque();
        }
        else if (axis.y > 0 && Input.GetMouseButtonDown(0))
        {
            if (Time.time - _lastMouse0PressedTime < .75f)
                _carTransmission.ToggleTransmission();
            _lastMouse0PressedTime = Time.time;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            _carController.HandBrake();
        }
        else if (Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1))
        {
            _carController.ReleaseHandBrake();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            _pauseButton.Click();

        if (Input.GetMouseButton(2))
            _carController.UseNitro();
        else if (Input.GetMouseButtonDown(2))
            _carController.StopNitro();

        if (Input.GetMouseButtonDown(3))
            _carTransmission.ShiftTo(false);
        else if (Input.GetMouseButtonDown(4))
            _carTransmission.ShiftTo(true);
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _carTransmission = _carController.GetComponent<CarTransmission>();
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private int ToInt(bool value) => value ? 1 : 0;
}
