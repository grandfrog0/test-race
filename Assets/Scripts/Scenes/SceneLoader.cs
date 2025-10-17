using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(int index)
    {
        Time.timeScale = 1;
        
        SceneLoadAnimation.Play(() => SceneManager.LoadSceneAsync(index));
    }
    public static void ReloadScene()
    {
        Time.timeScale = 1;
        SceneLoadAnimation.Play(() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }
}
