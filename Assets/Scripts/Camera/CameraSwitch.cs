using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������������ ������ ����������
/// </summary>
public class CameraSwitch : MonoBehaviour
{
    [SerializeField] List<CameraController> _controllers;
    private int _selectedIndex = 0;
    /// <summary>
    /// ����������� ��� ������
    /// </summary>
    public void Switch()
    {
        _controllers[_selectedIndex].enabled = false;
        _selectedIndex = ++_selectedIndex % _controllers.Count;
        _controllers[_selectedIndex].enabled = true;
    }
}
