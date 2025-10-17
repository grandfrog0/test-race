using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] UnityEvent _onFinished;
    // допустимый диапазон угла машины чтобы нельзя было проехать круг в обратную сторону
    [SerializeField] Vector2 _completeAngle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            float angle = Vector3.Angle(Vector3.forward, rb.velocity.normalized);
            if (angle > _completeAngle.x && angle < _completeAngle.y)
                _onFinished.Invoke();
            else
                SceneLoader.ReloadScene();
        }
    }
}
