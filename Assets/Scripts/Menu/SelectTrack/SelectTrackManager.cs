using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (SaveManager.Data.Tracks.Count == 0)
        {
            for(int i = 0; i < _tracks.Count; i++)
                SaveManager.Data.Tracks.Add(new TrackInfo() { Index = i + 1 });
            SaveManager.Data.Tracks[0].IsOpened = true;
        }

        for(int i = 0; i < _tracks.Count; i++)
        {
            _tracks[i].Index = SaveManager.Data.Tracks[i].Index;
            _tracks[i].IsUnlocked = SaveManager.Data.Tracks[i].IsOpened;
            _tracks[i].StarsCount = SaveManager.Data.Tracks[i].Stars;
            _tracks[i].BestTime = SaveManager.Data.Tracks[i].BestTime;
        }
    }
}
