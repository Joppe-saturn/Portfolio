using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSpawner : MonoBehaviour
{
    [SerializeField] private bool respawn;
    [SerializeField] private float respawnTime;

    [SerializeField] private GameObject bomb;

    private float timer;

    private GameObject newBomb;

    private void Start()
    {
        newBomb = Instantiate(bomb,transform.position,transform.rotation);
        timer = 0;
    }

    private void Update()
    {
        if (respawn && !newBomb.active)
        {
            if(timer > respawnTime)
            {
                timer = 0;
                newBomb = Instantiate(bomb, transform.position, transform.rotation);
            } else
            {
                timer += Time.deltaTime;
            }
        }
    }
}
