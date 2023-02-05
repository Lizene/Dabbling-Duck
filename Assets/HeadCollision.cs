using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    DuckHead headScript;
    float splitTime;
    int splitEveryXFood;
    GameManager gameManager;

    [SerializeField] private AudioClip[] goodFoodSound;
    [SerializeField] private AudioClip[] badFoodSound; 
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        headScript = GetComponentInParent<DuckHead>();
        splitTime = headScript.splitTime;
        splitEveryXFood = headScript.splitEveryXFood;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        print(splitTime);
        GetComponent<CircleCollider2D>().enabled = false;

        Invoke("EnableCollisions", splitTime);
    }
    void EnableCollisions()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Good Food"))
        {
            Destroy(collision.gameObject);
            headScript.foodSeen = false;
            gameManager.EatFood();
            PlayRandomClip(goodFoodSound);
            if (gameManager.foodEaten % splitEveryXFood == 0)
            {
                headScript.Split();
            }
        }
        else if (collision.gameObject.CompareTag("Bad Food"))
        {
            Destroy(collision.gameObject);
            headScript.foodSeen = false;
            gameManager.LoseLife();
            PlayRandomClip(badFoodSound);
        }
        else if (collision.gameObject.CompareTag("Head"))
        {
            print("Heads Collided");
            var otherHeadScript = collision.gameObject.GetComponentInParent<DuckHead>();
            var collisionNormal = (headScript.moveDir + otherHeadScript.moveDir).normalized;
            print(collisionNormal);
            headScript.moveDir = Vector2.Reflect(headScript.moveDir, collisionNormal);
            headScript.splitTimer = splitTime;
        }
    }
    private void PlayRandomClip(AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }
}
