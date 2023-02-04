using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    Transform canvas, hearts;
    TextMeshProUGUI text;
    public int lives;
    int maxLives;
    int foodEaten = 0;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        hearts = canvas.GetChild(0);
        text = canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        maxLives = lives;
    }

    void Update()
    {
        
    }
    public void GainLife()
    {
        if (lives >= maxLives) { return; }
        lives++;

        hearts.GetChild(6 + lives).gameObject.SetActive(true);
    }
    public void LoseLife()
    {
        lives--;
        hearts.GetChild(7 + lives).gameObject.SetActive(false);
        if (lives <= 0)
        {
            Die();
        }
    }
    public void EatFood()
    {
        foodEaten++;
        text.text = "Food Eaten: " + foodEaten.ToString();
    }
    public void Die()
    {
        canvas.GetChild(2).gameObject.SetActive(true);
    }
}
