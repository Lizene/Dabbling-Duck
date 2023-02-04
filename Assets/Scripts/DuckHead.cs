using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHead : MonoBehaviour
{
    public GameObject headPrefab, neckPrefab;
    public float baseControlledSpeed, mouseDetectRadius, turnSmoothTime;
    Camera cam;
    [System.NonSerialized] public Vector2 moveDir;
    [System.NonSerialized] public float splitTimer;
    Vector2 pos2;
    float currentVelocity;
    Transform detectCircle;
    DuckParent parentScript;
    bool moveEnabled = true;
    void Start()
    {
        detectCircle = transform.GetChild(0);
        detectCircle.localScale = Vector3.one * 2 * mouseDetectRadius;
        cam = Camera.main;
        parentScript = transform.GetComponentInParent<DuckParent>();
    }

    void Update()
    {
        Split();
        LookAtMouse();
        MoveToMouse();
    }
    void Split()
    {
        if (splitTimer > 0f) { splitTimer -= Time.deltaTime; }
        if (!(Input.GetKeyDown(KeyCode.Space) && parentScript.chosenChild == transform.GetSiblingIndex())) { return; }
        var newHead = Instantiate(headPrefab, transform.position, transform.rotation, transform.parent);
        newHead.name = "Duck Head "+newHead.transform.GetSiblingIndex().ToString();
        var headScript = newHead.GetComponent<DuckHead>();
        var newAngle = (transform.eulerAngles.z + 45f) * Mathf.Deg2Rad;
        moveDir = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
        headScript.moveDir = Vector2.Perpendicular(moveDir);
        splitTimer = 1.5f;
        headScript.splitTimer = 1.5f;
    }
    void LookAtMouse()
    {
        var mouseInput = cam.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = new Vector2(mouseInput.x, mouseInput.y);
        pos2 = new Vector2(transform.position.x, transform.position.y);
        var duckToCursor = cursorPos - pos2;
        moveEnabled = duckToCursor.magnitude > 0.5f;
        if (duckToCursor.magnitude <= mouseDetectRadius && splitTimer <= 0f) 
        {
            parentScript.chosenChild = transform.GetSiblingIndex();
            moveDir = duckToCursor.normalized;
        }
        var duckToCursorAngle = Vector2.SignedAngle(Vector2.up, moveDir);
        var smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, duckToCursorAngle, ref currentVelocity, turnSmoothTime);
        transform.eulerAngles = new Vector3(0, 0, smoothAngle);
    }
    void MoveToMouse()
    {
        if (!moveEnabled) { return; }
        var moveVector = moveDir * baseControlledSpeed * Time.deltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y, 0);
    }
}
