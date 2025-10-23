using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �������� ������
/// </summary>
public class RaceInfo : MonoBehaviour
{
    [SerializeField] UnityEvent<float> _onDriftValueChanged = new();
    [SerializeField] UnityEvent<float> _onVelocityChanged = new();
    [SerializeField] UnityEvent<float> _onScoreChanged = new();
    [SerializeField] UnityEvent<float> _onTimerChanged = new();
    [SerializeField] UnityEvent<float> _onBestChanged = new();
    [SerializeField] UnityEvent<float> _onNitroChanged = new();
    [SerializeField] UnityEvent _onCompleted = new();

    [SerializeField] int _trackIndex;
    /// <summary>
    /// ���������� ������ ������
    /// </summary>
    public float DriftValue { get; set; }
    /// <summary>
    /// ���������� ����� ������
    /// </summary>
    public float Score { get; set; }
    /// <summary>
    /// ����� ���������� ����� �� �����
    /// </summary>
    public float Total => (int)(TrackCoins + Score);
    /// <summary>
    /// ���������� ���������� ������
    /// </summary>
    public int Cycles { get; set; }
    /// <summary>
    /// ���������� ������������ ����
    /// </summary>
    public int StarsCount => Mathf.Clamp((int)Score / 250/*1000*/, 0, 3);
    /// <summary>
    /// ������� ����� ������
    /// </summary>
    public float Timer { get; private set; }
    /// <summary>
    /// ������ ��������� ������� �� ������
    /// </summary>
    public float Best { get; set; }
    [SerializeField] float _trackCoins = 200;
    /// <summary>
    /// ���� �� ����������� �����
    /// </summary>
    public float TrackCoins => _trackCoins;

    [SerializeField] CarController _car;
    private Vector2 _oldVelocity;
    private float _nitro;
    private bool _isDrifting = false;

    /// <summary>
    /// ���������� � ������ ���������� �����
    /// </summary>
    public void OnCycleFinished()
    {
        Cycles++;
        if (Cycles >= 3)
            Finish();
    }

    /// <summary>
    /// ���������� �� ��������� ������
    /// </summary>
    public void Finish()
    {
        Best = Best == 0 ? Timer : Mathf.Min(Timer, Best);
        FillData();
        _onCompleted.Invoke();
    }

    /// <summary>
    /// ��������� ��� ���������� � ������ �� ������������� �����
    /// </summary>
    private void FillData()
    {
        SaveManager.Data.Tracks[_trackIndex].Stars = StarsCount;
        SaveManager.Data.Tracks[_trackIndex].BestTime = Best;
        if (StarsCount > 1 && SaveManager.Data.Tracks.Any(x => x.Index == _trackIndex + 1)
            && _trackIndex + 1 < SaveManager.Data.Tracks.Count)
            SaveManager.Data.Tracks[_trackIndex + 1].IsOpened = true;
        SaveManager.SaveData();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _car.Velocity;
        if (_oldVelocity != velocity)
        {
            _onVelocityChanged.Invoke(velocity.magnitude);
            _oldVelocity = velocity;
        }

        if (_isDrifting != _car.IsDrifting)
        {
            _isDrifting = _car.IsDrifting;
            Debug.Log("smoke is active: " + _isDrifting);
            _car.Model.SetSmokeActive(_isDrifting);
        }

        if (_car.IsDrifting)
        {
            DriftValue += Time.fixedDeltaTime;
            _onDriftValueChanged.Invoke(DriftValue);
        }
        else if (DriftValue != 0)
        {
            DriftValue = 0;
            _onDriftValueChanged.Invoke(0);
        }

        if (DriftValue > 1)
        {
            Score += 11.45f * Time.fixedDeltaTime;
            _onScoreChanged.Invoke(Score);
        }

        Timer += Time.fixedDeltaTime * 100;
        _onTimerChanged.Invoke(Timer);

        if (_nitro != _car.Nitro)
        {
            _nitro = _car.Nitro;
            _onNitroChanged.Invoke(_nitro / 100);
        }
    }

    private void Start()
    {
        _onBestChanged.Invoke(Best);
    }
}
