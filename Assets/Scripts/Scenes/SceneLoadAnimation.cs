using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadAnimation : MonoBehaviour
{
    private static SceneLoadAnimation _instance;
    public static void Play(Func<AsyncOperation> func) => _instance.StartPlay(func);

    private Coroutine _animCoroutine;
    [SerializeField] Text _text;
    [SerializeField] Image _background;
    [SerializeField] Image _wheel;
    [SerializeField] Image _wheelBackground;
    private Color _wheelBackgroundColor = new Color(1f, 1f, 1f, 0.1f);

    public void Start()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else Destroy(gameObject);
    }

    private void StartPlay(Func<AsyncOperation> func)
    {
        if (!_animCoroutine.IsUnityNull())
            StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(PlayCoroutine(func));
    }
    private IEnumerator PlayCoroutine(Func<AsyncOperation> func)
    {
        _background.raycastTarget = true;
        _wheel.fillAmount = 0;
        for (float t = 0; t <= 1; t += Time.deltaTime * 2)
        {
            _background.color = Color.Lerp(Color.clear, Color.black, t);
            _text.color = _wheel.color = Color.Lerp(Color.clear, Color.white, t);
            _wheelBackground.color = Color.Lerp(Color.clear, _wheelBackgroundColor, t);
            yield return null;
        }
        _background.color = Color.black;
        _text.color = _wheel.color = Color.white;
        _wheelBackground.color = _wheelBackgroundColor;

        AsyncOperation op = func();
        for (float t = 0; t <= 1 || !op.isDone; t += Time.deltaTime)
        {
            _wheel.fillAmount = Mathf.Min(t, op.progress);
            yield return null;
        }

        for (float t = 0; t <= 1; t += Time.deltaTime * 2)
        {
            _background.color = Color.Lerp(Color.black, Color.clear, t);
            _text.color = _wheel.color = Color.Lerp(Color.white, Color.clear, t);
            _wheelBackground.color = Color.Lerp(_wheelBackgroundColor, Color.clear, t);
            yield return null;
        }
        _background.color = Color.clear;
        _text.color = _wheel.color = _wheelBackground.color  = Color.clear;
        _background.raycastTarget = false;
    }
}
