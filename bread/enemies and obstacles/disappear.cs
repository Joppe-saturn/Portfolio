using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;

    private void Start()
    {

    }

    private void Update()
    {
        if (player.hasCheckPointBeenReached)
        {
            wall2.SetActive(false);
            wall3.SetActive(true);
        }
    }
}
