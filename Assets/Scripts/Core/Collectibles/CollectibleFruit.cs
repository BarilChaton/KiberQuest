using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleFruit : MonoBehaviour
{
    public AudioClip pickupSound;
    public int scoreValue;
    public Text scoreText;

    private void Awake()
    {
        //scoreText = FindObjectOfType<Text>(scoreText);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Scoring.totalScore += scoreValue;
            scoreText.text = " " + Scoring.totalScore;
            gameObject.SetActive(false);
        }
    }
}
