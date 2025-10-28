using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за выбор трассы
/// </summary>
public class SelectTrackManager : MonoBehaviour
{
    [SerializeField] List<TrackView> _tracks;
    [SerializeField] GameObject _startButton;
    private TrackView _selectedTrack;
    public TrackView SelectedTrack
    {
        get => _selectedTrack;
        set
        {
            if (_selectedTrack != null)
                _selectedTrack.IsSelected = false;

            if (_selectedTrack != value)
            {
                if (value.IsUnlocked)
                {
                    _selectedTrack = value;
                    _selectedTrack.IsSelected = true;
                }
            }
            else _selectedTrack = null;

            _startButton.SetActive(_selectedTrack != null);
        }
    }
    public void SetSelectedIndex(int index) => SelectedTrack = _tracks[index];
    public void OpenSelectedTrack()
    {
        if (SelectedTrack.IsUnlocked)
            SceneLoader.LoadScene(SelectedTrack.Index);
    }

    private void Start()
    {
        UpdateTracks();
    }
    public void UpdateTracks()
    {
        if (SaveManager.Tracks.Count == 0)
        {
            for (int i = 0; i < _tracks.Count; i++)
            {
                string json = Resources.Load<TextAsset>("Configs/Tracks/track" + (i + 1)).text;
                SaveManager.Tracks.Add(JsonUtility.FromJson<TrackInfo>(json));
            }
        }

        for(int i = 0; i < _tracks.Count; i++)
        {
            _tracks[i].Index = SaveManager.Tracks[i].Index;
            _tracks[i].IsUnlocked = SaveManager.Tracks[i].IsOpened;
            _tracks[i].StarsCount = SaveManager.Tracks[i].StarsCount;
            _tracks[i].BestTime = SaveManager.Tracks[i].BestTime;
        }
    }
}
