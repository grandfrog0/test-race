using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceInfo : MonoBehaviour
{
    [SerializeField] UnityEvent<float> _onDriftValueChanged = new();
    [SerializeField] UnityEvent<float> _onVelocityChanged = new();
    [SerializeField] UnityEvent<float> _onScoreChanged = new();
    [SerializeField] UnityEvent<float> _onTimerChanged = new();
    [SerializeField] UnityEvent<float> _onBestChanged = new();
    [SerializeField] UnityEvent _onCompleted = new();
    public float DriftValue { get; set; }
    public float DriftCoins => Mathf.Max(DriftValue - 10, 0);
    public float Score { get; set; }
    public float Total => DriftCoins + Score;
    public int Cycles { get; set; }
    public int StarsCount => Mathf.Clamp((int)Score / 1000, 0, 3);
    public float Timer { get; set; }
    public float Best { get; set; }
    [SerializeField] float _trackCoins = 200;
    public float TrackCoins => _trackCoins;

    [SerializeField] CarController _car;
    private Vector2 _oldVelocity;

    public void OnCycleFinished()
    {
        Cycles++;
        if (Cycles >= 3)
            Finish();
    }

    public void Finish()
    {
        Best = Mathf.Min(Timer, Best);
        _onCompleted.Invoke();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _car.Velocity;
        if (_oldVelocity != velocity)
        {
            _onVelocityChanged.Invoke(velocity.magnitude);
            _oldVelocity = velocity;
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
    }

    private void Start()
    {
        _onBestChanged.Invoke(Best);
    }
}
