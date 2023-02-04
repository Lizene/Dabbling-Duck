using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    List<Transform> foods = new();
    DuckHead duckHeadScript;
    void Start()
    {
        duckHeadScript = GetComponentInParent<DuckHead>();
    }

    void Update()
    {
        for (int i = foods.Count-1; i >= 0; i--)
        {
            if (foods[i] == null) { foods.RemoveAt(i); }
        }
        if (foods.Count == 0)
        {
            duckHeadScript.foodSeen = false;
            return;
        }
        duckHeadScript.foodSeen = true;
        var closestIndex = 0;
        var closestDistance = (foods[0].position - transform.position).magnitude;
        for (int i = 1; i < foods.Count; i++)
        {
            var currentDistance = (foods[i].position - transform.position).magnitude;
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestIndex = i;
            }
        }
        duckHeadScript.targetedFood = foods[closestIndex];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foods.Add(collision.transform);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foods.Remove(collision.transform);
    }
}
