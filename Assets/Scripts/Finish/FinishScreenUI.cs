using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] RaceInfo _raceInfo;

    [SerializeField] TimerText _currentText, _bestText;
    [SerializeField] FormattedText _track, _driftCoins, _total;

    public void OnFinished()
    {
        _currentText.SetTime(_raceInfo.Timer);
        _bestText.SetTime(_raceInfo.Best);

        _track.SetValue(_raceInfo.TrackCoins);
        _driftCoins.SetValue(_raceInfo.DriftCoins);
        _total.SetValue(_raceInfo.Total);
    }
}
