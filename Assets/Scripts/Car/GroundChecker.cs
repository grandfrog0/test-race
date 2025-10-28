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
    private Rigidbody _rigidbody;

    public bool IsOnGround { get; private set; }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + _carOffset, -transform.up);

        if ((IsOnGround = Physics.Raycast(ray, 3, _groundMask)))
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
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
