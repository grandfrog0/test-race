using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �������� ������ ����������
/// </summary>
public class CarModel : MonoBehaviour
{
    [SerializeField] List<Renderer> _wheelsRenderers;
    [SerializeField] ParticleSystem _smokePrefab;
    private List<ParticleSystem> _smokeParticles;
    public WheelCollider WheelBL, WheelBR, WheelFL, WheelFR;

    public void Initialize(CarInfo info)
    {
        _smokeParticles = new();
        foreach (WheelCollider c in new List<WheelCollider>() { WheelBL, WheelBR, WheelFL, WheelFR })
        {
            _smokeParticles.Add(Instantiate(_smokePrefab, c.transform.position, c.transform.rotation, c.transform));
        }

        SetWheelsColor(info.WheelColor);
        SetSmokeColor(info.SmokeColor);
    }
    /// <summary>
    /// ������ ���� �����
    /// </summary>
    /// <param name="color">����� ����</param>
    public void SetWheelsColor(Color color)
    {
        foreach (Renderer r in _wheelsRenderers)
        {
            r.material.color = color;
        }
    }
    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="color">����� ����</param>
    public void SetSmokeColor(Color color)
    {
        foreach(ParticleSystem p in _smokeParticles)
        {
            p.startColor = color == Color.clear ? Color.black : color;
        }
    }
    /// <summary>
    /// ��������/��������� ������ ���� �� �����
    /// </summary>
    /// <param name="value"></param>
    public void SetSmokeActive(bool value)
    {
        if (value == true)
        {
            foreach (ParticleSystem p in _smokeParticles)
            {
                p.Play();
            }
        }
        else
        {
            foreach (ParticleSystem p in _smokeParticles)
            {
                p.Stop();
            }
        }
    }
}
