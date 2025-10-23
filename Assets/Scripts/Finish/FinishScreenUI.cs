using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет UI-элементами на экране окончания забега
/// </summary>
public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] RaceInfo _raceInfo;

    [SerializeField] TimerText _currentText, _bestText;
    [SerializeField] FormattedText _track, _driftCoins, _total;
    [SerializeField] List<Image> _starsImages;

    public void OnFinished()
    {
        _currentText.SetTime(_raceInfo.Timer);
        _bestText.SetTime(_raceInfo.Best);

        _track.SetValue(_raceInfo.TrackCoins);
        _driftCoins.SetValue((int)_raceInfo.Score);
        _total.SetValue(_raceInfo.Total);

        for(int i = 0; i < _raceInfo.StarsCount; i++)
        {
            _starsImages[i].color = Color.white;
        }
    }
}
