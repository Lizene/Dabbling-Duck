using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckParent : MonoBehaviour
{
    public int chosenChild;
    void Start()
    {
        chosenChild = 0;
        transform.GetChild(0).GetComponent<DuckHead>().moveDir = Vector2.down;
    }

    void Update()
    {
        
    }
}
