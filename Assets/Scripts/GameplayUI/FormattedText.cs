using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormattedText : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] string _format;
    public void SetValue(float value)
        => _text.text = string.Format(_format, value);
    public void SetValue(int value)
        => _text.text = string.Format(_format, value);
}
