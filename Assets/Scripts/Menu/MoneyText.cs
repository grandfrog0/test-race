using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Текст, отображающий текущую денежную сумму
/// </summary>
public class MoneyText : MonoBehaviour
{
    [SerializeField] Text _text;
    private void SetValue(int _, int value) => _text.text = value.ToString();
    private void Start()
    {
        SetValue(0, MoneyManager.MoneyCount);
        MoneyManager.OnMoneyCountChanged += SetValue;
    }
    private void OnDestroy()
    {
        MoneyManager.OnMoneyCountChanged -= SetValue;
    }
}
