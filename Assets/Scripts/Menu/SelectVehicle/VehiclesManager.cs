using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за настройку автомобилей
/// </summary>
public class VehiclesManager : MonoBehaviour
{
    public static CarModel CurModel { get; private set; }

    [SerializeField] List<CarConfig> _carConfigs;
    [SerializeField] Transform _modelParent;

    [SerializeField] Text _carNameText;
    [SerializeField] GameObject _buyButton, _customizeButton;
    public int SelectedIndex { get; private set; }

    public void Start()
    {
        _carConfigs = PrefabBuffer.instance.DefaultCarConfigs;

        SelectedIndex = SaveManager.PlayerData.SelectedCarIndex;

        if (SaveManager.Cars.Count == 0)
            SaveManager.Cars.AddRange(_carConfigs.Select(x => x.Info.Clone()));
        SaveManager.Cars[0].IsOpened = true;

        UpdateModel();
    }
    public void Next()
    {
        SelectedIndex = ++SelectedIndex % _carConfigs.Count;
        UpdateModel();
    }
    public void Previous()
    {
        SelectedIndex = --SelectedIndex;
        while (SelectedIndex < 0)
            SelectedIndex += _carConfigs.Count;
        UpdateModel();
    }
    public void UpdateModel()
    {
        if (CurModel)
            Destroy(CurModel.gameObject);

        CurModel = Instantiate(SaveManager.Cars[SelectedIndex].Model, _modelParent).GetComponent<CarModel>();
        CurModel.Initialize(SaveManager.Cars[SelectedIndex]);
        _carNameText.text = SaveManager.Cars[SelectedIndex].Title;

        _buyButton.gameObject.SetActive(!SaveManager.Cars[SelectedIndex].IsOpened);
        _customizeButton.gameObject.SetActive(SaveManager.Cars[SelectedIndex].IsOpened);

        if (SaveManager.Cars[SelectedIndex].IsOpened) 
            SaveManager.PlayerData.SelectedCarIndex = SelectedIndex;
    }

    public void TryBuyVehicle()
    {
        if (SaveManager.Cars[SelectedIndex].IsOpened || MoneyManager.MoneyCount < 3000)
            return;

        SaveManager.Cars[SelectedIndex].IsOpened = true;
        MoneyManager.MoneyCount -= 3000;
        UpdateModel();
    }
}
