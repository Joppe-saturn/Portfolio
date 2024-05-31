using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] public GameObject player;

    public float score;
    
    private void Start()
    {

    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void scoreUpdate(float amount)
    {
        if(player != null)
        {
            score += amount;
        }
    }
}
