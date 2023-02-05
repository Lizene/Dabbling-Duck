using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHead : MonoBehaviour
{
    public GameObject headPrefab, neckPrefab;
    public Sprite closedHeadSprite, openHeadSprite;
    public float baseControlledSpeed, baseUncontrolledSpeed, detectRadius, turnSmoothTime, splitTime;
    public bool splitWithSpace, firstHead;
    public int splitEveryXFood;

    [System.NonSerialized] public Vector2 moveDir;
    [System.NonSerialized] public float splitTimer;
    [System.NonSerialized] public Transform targetedFood;
    [System.NonSerialized] public bool foodSeen;

    Camera cam;
    Transform detectCircle;
    DuckParent parentScript;
    GameManager gameManager;
    SpriteRenderer spriteRend;

    Vector2 pos2;

    bool moveEnabled = true, controlled, headOpen, headOpenLastFrame;
    const float screenHorizontal = 15f, screenDown = 7.5f, screenUp = 6.6f;
    float smoothTurnCurrentVelocity;
    
    void Start()
    {
        detectCircle = transform.GetChild(0);
        detectCircle.localScale = Vector3.one * 2 * detectRadius;
        cam = Camera.main;
        parentScript = transform.GetComponentInParent<DuckParent>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = closedHeadSprite;
        moveDir = Vector2.down;
        splitTimer = splitTime;
    }

    void Update()
    {
        SplitInput();
        Look();
        Move();
    }
    void SplitInput()
    {
        if (!splitWithSpace) { return; }
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        if (parentScript.chosenChild == transform.GetSiblingIndex()) { Split(); }
    }
    void Look()
    {
        var mouseInput = cam.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = new Vector2(mouseInput.x, mouseInput.y);
        pos2 = new Vector2(transform.position.x, transform.position.y);
        var duckToCursor = cursorPos - pos2;
        moveEnabled = true;
        headOpen = false;
        splitTimer -= Time.deltaTime;
        controlled = false;
        if (splitTimer <= 0f)
        {
            if (duckToCursor.magnitude <= detectRadius && !isOutOfBounds())
            {
                if (duckToCursor.magnitude < 0.5f) { moveEnabled = false; }
                controlled = true;
                parentScript.chosenChild = transform.GetSiblingIndex();
                moveDir = duckToCursor.normalized;
            }
            else
            {
                if (targetedFood == null) { foodSeen = false; }
                if (foodSeen)
                {
                    var duckToFood = targetedFood.transform.position - transform.position;
                    moveDir = duckToFood.normalized;
                }
            }
        }
        if (targetedFood == null) { foodSeen = false; }
        if (foodSeen)
        {
            var duckToFood = targetedFood.transform.position - transform.position;
            if (duckToFood.magnitude < 1f)
            {
                headOpen = true;
            }
        }
        if (headOpen != headOpenLastFrame)
        {
            spriteRend.sprite = headOpen ? openHeadSprite : closedHeadSprite;
        }
        headOpenLastFrame = headOpen;
        var duckToCursorAngle = Vector2.SignedAngle(Vector2.up, moveDir);
        var smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, duckToCursorAngle, ref smoothTurnCurrentVelocity, turnSmoothTime);
        transform.eulerAngles = new Vector3(0, 0, smoothAngle);
    }
    void Move()
    {
        if (!moveEnabled) { return; }
        var moveVector = moveDir * (splitTimer < 0f ? (controlled ? baseControlledSpeed : baseUncontrolledSpeed) : baseControlledSpeed) * Time.deltaTime;
        var moveVector3 = new Vector3(moveVector.x, moveVector.y, 0);
        transform.position += moveVector3;
        if (isOutOfBounds())
        {
            if (!controlled)
            {
                var pos = transform.position;
                if (pos.x < -screenHorizontal || pos.x > screenHorizontal)
                {
                    moveDir = Vector2.Reflect(moveDir, Vector2.right);
                }
                if (pos.y < -screenDown || pos.y > screenUp)
                {
                    moveDir = Vector2.Reflect(moveDir, Vector2.up);
                }
            }
            transform.position -= moveVector3;
        }
    }
    public void Split()
    {
        if (splitTimer > 0f) { splitTimer -= Time.deltaTime; }
        var newHead = Instantiate(headPrefab, transform.position, transform.rotation, transform.parent);
        newHead.name = "Duck Head " + newHead.transform.GetSiblingIndex().ToString();
        var headScript = newHead.GetComponent<DuckHead>();
        var newAngle = (transform.eulerAngles.z + 45f) * Mathf.Deg2Rad;
        moveDir = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
        headScript.moveDir = Vector2.Perpendicular(moveDir);
        splitTimer = splitTime;
    }

    bool isOutOfBounds()
    {
        var pos = transform.position;
        return pos.x < -screenHorizontal || pos.x > screenHorizontal || pos.y < -screenDown || pos.y > screenUp;
    }
}
