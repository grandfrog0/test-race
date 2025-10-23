using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  од дл€ проверки трассы под ногами. ¬ случае вылета с трассы возвращает обратно.
/// </summary>
public class GroundChecker : MonoBehaviour
{
    [SerializeField] Vector3 _carOffset;
    [SerializeField] LayerMask _groundMask;
    private float _lastAngle;
    private Vector3 _lastPos;
    private float _outsideTime;

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + _carOffset, -transform.up);

        Debug.Log(Physics.Raycast(ray, 5, _groundMask));
        if (Physics.Raycast(ray, 5, _groundMask))
        {
            _lastAngle = transform.eulerAngles.y;
            _lastPos = transform.position;
            _outsideTime = 0;
        }
        else
        {
            _outsideTime += Time.fixedDeltaTime;
            if (_outsideTime >= 10)
            {
                transform.position = _lastPos;
                transform.rotation = Quaternion.Euler(0, _lastAngle, 0);
            }
        }
    }
}
