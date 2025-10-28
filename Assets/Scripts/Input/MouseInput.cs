
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
    private bool _isHandBroken;

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

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            _carController.HandBrake();
            _isHandBroken = true;
        }
        else if (_isHandBroken)
        {
            _carController.ReleaseHandBrake();
            _isHandBroken = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            _pauseButton.Click();

        if (Input.GetMouseButton(2))
            _carController.UseNitro();
        else if (Input.GetMouseButtonDown(2))
            _carController.StopNitro();
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private int ToInt(bool value) => value ? 1 : 0;
}
