using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    private bool hasBossDiedYet = true;
    public bool isBossActive = false;
    private bool fightIsActive = false;

    [SerializeField] private List<float> randomTimeUntilBossSpawns = new List<float>();
    private float bossTimer;

    [SerializeField] private float speed;

    [SerializeField] private float moveIntensity;
    private float timer;

    [SerializeField] private float bossHealth;
    private float currentBossHealth;

    [SerializeField] private GameObject boem;

    private GM gameManager;

    [SerializeField] private GameObject ufoObject;

    private void Start()
    {
        gameManager = FindObjectOfType<GM>();
    }

    private void Update()
    {
        if(hasBossDiedYet)
        {
            hasBossDiedYet = false;
            isBossActive = false;
            bossTimer = Random.Range(randomTimeUntilBossSpawns[0], randomTimeUntilBossSpawns[1]);
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(bossTimer);
        bossTimer -= 1;

        if (bossTimer < 0 && !isBossActive)
        {
            isBossActive = true;
            currentBossHealth = bossHealth;
        }

        if (isBossActive && !fightIsActive)
        {
            transform.position += new Vector3(0, 0, -0.2f);
            if (transform.position.z <= 8)
            {
                fightIsActive = true;
            }
        }

        if (fightIsActive)
        {
            timer += 0.05f;
            transform.position += new Vector3(Mathf.Cos(timer * speed) * moveIntensity, transform.position.y, Mathf.Sin(timer * speed) * Mathf.Sin(timer * 2 * speed) * (moveIntensity / 10));
            if(Random.Range(0, 100) == 69)
            {
                Instantiate(ufoObject, transform.position, transform.rotation);
            }
        }

        if(currentBossHealth < 0)
        {
            hasBossDiedYet = true;
            isBossActive = false;
            fightIsActive = false;
            for(int i = 0; i < 20; i++) 
            { 
                Instantiate(boem, new Vector3(transform.position.x + Random.Range(5.0f, -5.0f), transform.position.y + Random.Range(1.0f, -1.0f), transform.position.z + Random.Range(5.0f, -5.0f)), transform.rotation);
            }
            transform.position = new Vector3(0, 0, 20);
            currentBossHealth = 0;
            gameManager.scoreUpdate(20000);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bullet")
        {
            Destroy(other.gameObject);
            currentBossHealth -= 50;
            gameManager.scoreUpdate(150);
        }
    }
}
