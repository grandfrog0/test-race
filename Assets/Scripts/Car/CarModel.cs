using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CarModel : MonoBehaviour
{
    [SerializeField] List<Renderer> _wheelsRenderers;
    public void Initialize(CarInfo info)
    {
        SetWheelsColor(info.WheelColor);
        SetSmokeColor(info.SmokeColor);
    }
    public void SetWheelsColor(Color color)
    {
        foreach (Renderer r in _wheelsRenderers)
        {
            r.material.color = color;
        }
    }
    public void SetSmokeColor(Color color)
    {
        Debug.Log("TODO: Smoke color is " + color);
    }
}
