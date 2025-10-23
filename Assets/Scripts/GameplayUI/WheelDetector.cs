using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт детектора заноса автомобиля
/// </summary>
public class WheelDetector : MonoBehaviour
{
    [SerializeField] Image _image;
    public void SetValue(float value)
    {
        _image.fillAmount = value;
    }
}
