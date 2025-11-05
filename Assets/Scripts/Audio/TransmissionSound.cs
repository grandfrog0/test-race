using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionSound : MonoBehaviour
{
    [SerializeField] AudioSource _idle, _drive;
    public void SetRevolutions(float value)
    {
        if (Mathf.Abs(value) < 10)
        {
            if (!_idle.isPlaying)
            {
                _idle.Play();
                _drive.Stop();
            }
        }
        else
        {
            if (!_drive.isPlaying)
            {
                _idle.Stop();
                _drive.Play();
            }
            _drive.pitch = Mathf.Clamp(value / 1000, 1, 3);
        }
    }
}
