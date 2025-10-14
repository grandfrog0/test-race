using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackView : MonoBehaviour
{
    [SerializeField] GameObject _lock;
    [SerializeField] List<Image> _stars;
    [SerializeField] Color _starActiveColor, _starDisactiveColor;
    [SerializeField] Text _bestTimeText;
    [SerializeField] Button _button;

    private bool _isUnlocked;
    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            _isUnlocked = value;
            _lock.SetActive(!value);
            foreach (Image star in _stars)
                star.gameObject.SetActive(value);
            _button.enabled = value;
        }
    }
    private int _starsCount;
    public int StarsCount
    {
        get => _starsCount;
        set
        {
            _starsCount = value;
            for(int i = 0; i < _stars.Count; i++)
                _stars[i].color = i < _starsCount ? _starDisactiveColor : _starActiveColor;
        }
    }
    private float _bestTime;
    public float BestTime
    {
        get => _bestTime;
        set
        {
            _bestTime = value;
            int ms = (int)_bestTime % 100;
            int s = (int)_bestTime % 600 - ms;
            int m = (int)_bestTime / 3600;
            _bestTimeText.text = $"BEST : {m}:{s}.{ms}";
        }
    }
    public int Index { get; set; }
    public void OnClick()
    {
        if (IsUnlocked)
            SceneLoader.LoadScene(Index);
    }
}
