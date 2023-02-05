using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuClick : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneLoader.loadScene("Game Scene");
        }
    }
}
