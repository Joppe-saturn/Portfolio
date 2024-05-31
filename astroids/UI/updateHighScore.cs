using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class updateHighScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        
    }

    private void Update()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
    }
}
