using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GearText : MonoBehaviour
{
    [SerializeField] Text _text;
    public void SetValue(float value)
        => _text.text = value == -1 ? "R" : value == 0 ? "N" : value.ToString();
}
