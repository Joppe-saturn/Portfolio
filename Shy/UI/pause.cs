using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.active)
            {
                pauseScreen.SetActive(false);
                player.GetComponent<NavMeshAgent>().enabled = true;
            }
            else
            {
                pauseScreen.SetActive(true);
                player.GetComponent<NavMeshAgent>().enabled = false;
            }
        }
    }
}
