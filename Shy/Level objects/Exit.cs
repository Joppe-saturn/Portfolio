using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float winDistance;
    [SerializeField] private string nextLevel;

    void Start()
    {
        
    }

    void Update()
    {
        if((transform.position - player.transform.position).magnitude < winDistance)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
