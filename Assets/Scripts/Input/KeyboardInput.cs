using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за обработку нажатий на клавиатуре
/// </summary>
public class KeyboardInput : MonoBehaviour
{
    [SerializeField] CarController _carController;
    [SerializeField] PauseButton _pauseButton;
    private CarTransmission _carTransmission;

    public void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _carController.SetAxis(axis);
        if (axis.y < 0)
        {
            if (Input.GetButton("Vertical"))
                _carController.BrakeTorque();
            if (Input.GetButtonUp("Vertical"))
                _carController.ReleaseTorque();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _carTransmission.ToggleTransmission();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _carTransmission.CurrentGear = -1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _carController.HandBrake();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            _carController.ReleaseHandBrake();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            _pauseButton.Click();

        if (Input.GetKey(KeyCode.LeftShift))
            _carController.UseNitro();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _carController.StopNitro();

        if (Input.GetKeyDown(KeyCode.Q))
            _carTransmission.ShiftTo(false);
        else if (Input.GetKeyDown(KeyCode.E))
            _carTransmission.ShiftTo(true);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            _carTransmission.IsClutchPressed = true;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            _carTransmission.IsClutchPressed = false;

    }

    private void Start()
    {
        _carTransmission = _carController.GetComponent<CarTransmission>();
    }
}
