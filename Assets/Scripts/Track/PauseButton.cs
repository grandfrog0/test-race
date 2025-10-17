using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PauseButton : MonoBehaviour
{
    [SerializeField] UnityEvent _onPause = new();
    [SerializeField] UnityEvent _onResume = new();
    public void Click()
    {
        if (Time.timeScale == 0)
        {
            _onResume.Invoke();
            Resume();
        }
        else
        {
            _onPause.Invoke();
            Pause();
        }
    }
    public void Pause() => Time.timeScale = 0;
    public void Resume() => Time.timeScale = 1;
}
