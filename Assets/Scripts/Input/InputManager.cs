using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] KeyboardInput _keyboardInput;
    [SerializeField] MouseInput _mouseInput;
    private void Awake()
    {
        bool isKeyboardActive = SaveManager.SettingsData.IsKeyboardOn;
        _keyboardInput.enabled = isKeyboardActive;
        _mouseInput.enabled = !isKeyboardActive;
    }
}
