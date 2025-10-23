using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// При старте выбирает случайную трассу из доступных
/// </summary>
public class RandomTrack : MonoBehaviour
{
    [SerializeField] List<GameObject> _tracks;
    private void Start()
    {
        _tracks[Random.Range(0, _tracks.Count)].SetActive(true);
    }
}
