using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
