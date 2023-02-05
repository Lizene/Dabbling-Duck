using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHead : MonoBehaviour
{
    public GameObject headPrefab, neckPrefab;
    public Sprite openHeadSprite;
    public float baseControlledSpeed, baseUncontrolledSpeed, detectRadius, turnSmoothTime;
    Camera cam;
    [System.NonSerialized] public Vector2 moveDir;
    [System.NonSerialized] public float splitTimer;
    [System.NonSerialized] public Transform targetedFood;
    [System.NonSerialized] public bool foodSeen;
    Vector2 pos2;
    float currentVelocity;
    Transform detectCircle;
    DuckParent parentScript;
    bool moveEnabled = true, controlled;
    const float screenHorizontal = 15f;
    const float screenDown = 7.5f;
    const float screenUp = 6.6f;
    GameManager gameManager;
    SpriteRenderer spriteRend;
    private bool headOpen;
    private bool headOpenLastFrame;
    Sprite closedHeadSprite;

    [SerializeField] private AudioClip[] goodFoodSound;
    [SerializeField] private AudioClip[] badFoodSound; 
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        detectCircle = transform.GetChild(0);
        detectCircle.localScale = Vector3.one * 2 * detectRadius;
        cam = Camera.main;
        parentScript = transform.GetComponentInParent<DuckParent>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spriteRend = GetComponent<SpriteRenderer>();
        closedHeadSprite = spriteRend.sprite;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Split();
        Look();
        Move();
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
    bool isOutOfBounds()
    {
        var pos = transform.position;
        return pos.x < -screenHorizontal || pos.x > screenHorizontal || pos.y < -screenDown || pos.y > screenUp;
    }
    void Look()
    {
        var mouseInput = cam.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = new Vector2(mouseInput.x, mouseInput.y);
        pos2 = new Vector2(transform.position.x, transform.position.y);
        var duckToCursor = cursorPos - pos2;
        moveEnabled = true;
        headOpen = false;
        if (duckToCursor.magnitude <= detectRadius && splitTimer <= 0f && Input.GetMouseButton(0) && !isOutOfBounds()) 
        {
            if (duckToCursor.magnitude < 0.5f) { moveEnabled = false; }
            controlled = true;
            parentScript.chosenChild = transform.GetSiblingIndex();
            moveDir = duckToCursor.normalized;
        }
        else
        {
            controlled = false;
            if (targetedFood == null) { foodSeen = false; }
            if (foodSeen)
            {
                var duckToFood = targetedFood.transform.position - transform.position;
                moveDir = duckToFood.normalized;
            }
        }
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
        var smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, duckToCursorAngle, ref currentVelocity, turnSmoothTime);
        transform.eulerAngles = new Vector3(0, 0, smoothAngle);
    }

    void Move()
    {
        if (!moveEnabled) { return; }
        var moveVector = moveDir * (controlled?baseControlledSpeed:baseUncontrolledSpeed) * Time.deltaTime;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Good Food"))
        {
            Destroy(collision.gameObject);
            foodSeen = false;
            gameManager.EatFood();
            PlayRandomClip(goodFoodSound);
        }
        else if (collision.gameObject.CompareTag("Bad Food"))
        {
            Destroy(collision.gameObject);
            foodSeen = false;
            gameManager.LoseLife();
            PlayRandomClip(badFoodSound);
        }
    }
    private void PlayRandomClip(AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }
}
