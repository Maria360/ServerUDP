using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayer : MonoBehaviour
{
    public string Id { get; set; }
    public string Username { get; set; }
    public int Score { get; set; }

    public TMP_Text usertext;
    public TMP_Text scoreText;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        usertext = GetComponentInChildren<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();
        //scoreText = GetComponentInChildren<TMP_Text>();


    }

}
