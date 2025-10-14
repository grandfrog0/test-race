using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _driveSpeed, _rotateSpeed, _brakeStrength;
    [SerializeField] WheelCollider frontLeftWheel, frontRightWheel;
    [SerializeField] WheelCollider backLeftWheel, backRightWheel;
    private void Update()
    {
        float speed = Input.GetAxis("Vertical") * _driveSpeed;
        frontLeftWheel.motorTorque = speed;
        frontRightWheel.motorTorque = speed;
        backLeftWheel.motorTorque = speed;
        backRightWheel.motorTorque = speed;

        float angle = Input.GetAxis("Horizontal") * _rotateSpeed;
        frontLeftWheel.steerAngle = angle;
        frontRightWheel.steerAngle = angle;

        if (Input.GetAxis("Vertical") < 0)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                frontLeftWheel.brakeTorque = _brakeStrength;
                frontRightWheel.brakeTorque = _brakeStrength;
                backLeftWheel.brakeTorque = _brakeStrength;
                backRightWheel.brakeTorque = _brakeStrength;
            }
            else if (Input.GetButtonUp("Vertical"))
            {
                frontLeftWheel.brakeTorque = 0;
                frontRightWheel.brakeTorque = 0;
                backLeftWheel.brakeTorque = 0;
                backRightWheel.brakeTorque = 0;
            }
        }
    }
}
