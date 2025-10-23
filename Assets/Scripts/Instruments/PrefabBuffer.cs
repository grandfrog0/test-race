using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Содержит необходимые для сцены префабы
/// </summary>
public class PrefabBuffer : MonoBehaviour
{
    public static PrefabBuffer instance;

    public List<CarConfig> DefaultCarConfigs;
    public List<Color> CarColors;

    private void Awake()
    {
        instance = this;
    }
}
