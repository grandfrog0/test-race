using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт кнопки выхода из игры
/// </summary>
public class ExitButton : MonoBehaviour
{
    public void Quit() => Application.Quit();
}
