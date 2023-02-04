using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHead : MonoBehaviour
{
    public GameObject neckPrefab;
    public float baseControlledSpeed, mouseDetectRadius, turnSmoothTime;
    Camera cam;
    Vector2 pos2, moveDir;
    float currentVelocity;
    Transform detectCircle;
    GameObject neck;
    LineRenderer neckRend;
    List<Vector3> neckPoints;

    void Start()
    {
        detectCircle = transform.GetChild(0);
        detectCircle.localScale = Vector3.one * 2 * mouseDetectRadius;
        cam = Camera.main;
        moveDir = Vector2.down;
        neck = Instantiate(neckPrefab,GameObject.Find("Necks").transform);
        neckRend = neck.GetComponent<LineRenderer>();
        neckPoints = new List<Vector3>();
        neckPoints.Add(neckRend.GetPosition(0));
        InvokeRepeating("AddNeckPoint", 0.2f, 0.2f);
    }

    void Update()
    {
        LookAtMouse();
        MoveToMouse();
    }

    void LookAtMouse()
    {
        var mouseInput = cam.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = new Vector2(mouseInput.x, mouseInput.y);
        pos2 = new Vector2(transform.position.x, transform.position.y);
        var duckToCursor = cursorPos - pos2;
        if (duckToCursor.magnitude <= mouseDetectRadius) 
        {
            moveDir = duckToCursor.normalized;
        }
        var duckToCursorAngle = Vector2.SignedAngle(Vector2.up, moveDir);
        var smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, duckToCursorAngle, ref currentVelocity, turnSmoothTime);
        transform.eulerAngles = new Vector3(0, 0, smoothAngle);
    }
    void MoveToMouse()
    {
        var moveVector = moveDir * baseControlledSpeed * Time.deltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y, 0);
    }
    void AddNeckPoint()
    {
        neckPoints.Add(new Vector3(pos2.x, pos2.y, 0));
        print(neckPoints.Count);
        neckRend.SetPositions(neckPoints.ToArray());
    }
}
