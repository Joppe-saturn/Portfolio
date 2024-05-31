using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class countHighScore : MonoBehaviour
{
    private GM gameManager;
    private updateHighScore highScoreCounter;
    private movement player;

    private bool gameIsGoing = false;
    private List<float> scores = new List<float>();

    private string scoreText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        for(int i = 0; i < 5; i++)
        {
            scores.Add(0);
        }
    }

    private void Update()
    {
        if (gameManager == null && FindObjectOfType<GM>() != null)
        {
            gameManager = FindObjectOfType<GM>();
        }
        
        if(highScoreCounter == null && FindObjectOfType<updateHighScore>() != null)
        {
            highScoreCounter = FindObjectOfType<updateHighScore>();
        }

        if (player == null && FindObjectOfType<movement>() != null)
        {
            player = FindObjectOfType<movement>();
        }

        scoreText = "";
        for (int i = 0; i < 5; i++)
        {
            scoreText = scoreText + (i + 1) + ". " + scores[i] + "\n\n";
        }

        if(highScoreCounter != null && highScoreCounter.highScoreText != null)
        {
            highScoreCounter.highScoreText.text = scoreText;
        }

        if (player != null)
        {
            if (gameIsGoing && player.lives < 1)
            {
                gameIsGoing = false;
                scores.Add(gameManager.score);

                scores.Sort();
                scores.Reverse();
            }
            else if (player.lives > 0)
            {
                gameIsGoing = true;
            }
        }
    }
}
