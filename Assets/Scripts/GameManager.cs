using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject explosion;
    Transform canvas, hearts, heads;
    TextMeshProUGUI text;
    public int lives, foodEaten;
    int maxLives;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        heads = GameObject.Find("Duck Heads").transform;
        hearts = canvas.GetChild(0);
        text = canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        maxLives = lives;
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
        foreach (DuckHead script in heads.GetComponentsInChildren<DuckHead>())
        {
            Instantiate(explosion, script.transform.position-Vector3.forward, Quaternion.identity);
            script.enabled = false;
        }
    }
}
