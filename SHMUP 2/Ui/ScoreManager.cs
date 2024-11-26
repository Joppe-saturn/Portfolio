using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + player.score;
    }
}
