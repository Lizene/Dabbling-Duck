using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFoods : MonoBehaviour
{
    List<Vector2> points;
    List<GameObject> prefabList = new List<GameObject>();
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;
    public GameObject Prefab5;
    public GameObject Prefab6;
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    void Start()
    {
        prefabList.Add(Prefab1);
        prefabList.Add(Prefab2);
        prefabList.Add(Prefab3);
        prefabList.Add(Prefab4);
        prefabList.Add(Prefab5);
        prefabList.Add(Prefab6);

        points = PoissonDisc.GeneratePoints(radius, regionSize, rejectionSamples);

        if (points != null){
            foreach (Vector2 point in points) {
                int prefabIndex = UnityEngine.Random.Range(0,5);
                Vector3 pointPos = new Vector3 (point.x,point.y,0);
                Vector2 foodPos = transform.position + pointPos;
                Instantiate(prefabList[prefabIndex], foodPos, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
