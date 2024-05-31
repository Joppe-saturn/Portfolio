using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public float score;

    private void Start()
    {
        
    }

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }
}
