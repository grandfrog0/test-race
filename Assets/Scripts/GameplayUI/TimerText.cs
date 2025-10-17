using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    [SerializeField] Text _text;
    public string _formatString;

    public void SetTime(float time)
    {
        float ms = time % 100;
        float s = (time % 6000 - ms) / 100;
        float m = (time - s * 100 - ms) / 6000;
        _text.text = string.Format(_formatString, ms, s, m);
    }
}
