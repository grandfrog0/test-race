using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт переключения камеры автомобиля
/// </summary>
public class CameraSwitch : MonoBehaviour
{
    [SerializeField] List<CameraController> _controllers;
    private int _selectedIndex = 0;
    /// <summary>
    /// Переключить вид камеры
    /// </summary>
    public void Switch()
    {
        _controllers[_selectedIndex].enabled = false;
        _selectedIndex = ++_selectedIndex % _controllers.Count;
        _controllers[_selectedIndex].enabled = true;
    }
}
