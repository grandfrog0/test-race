using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за обработку нажатий на клавиатуре
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] CarController _carController;
    [SerializeField] PauseButton _pauseButton;
    public void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _carController.SetAxis(axis);
        if (axis.y < 0)
        {
            if (Input.GetButtonDown("Vertical"))
                _carController.BrakeTorque();
            if (Input.GetButtonUp("Vertical"))
                _carController.ReleaseTorque();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            _pauseButton.Click();

        if (Input.GetKey(KeyCode.LeftShift))
            _carController.UseNitro();

    }
}
