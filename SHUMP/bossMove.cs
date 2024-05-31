using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] List<GameObject> enemiesSpawn;
    [SerializeField] private List<int> enemySpawnChance;
    [SerializeField] private float enemyCooldown;
    private float enemyTimer;

    [SerializeField] private Vector2 startPos;

    [SerializeField] private GameObject bullet;
    [SerializeField] private List<Vector2> bulletSummon;
    [SerializeField] private int shootingSpeed;
    [SerializeField] private int shootingCooldown;
    private float shootingTimer;

    [SerializeField] private float lives;
    [SerializeField] private float invisibiltyAfterHit;
    private float hitTimer;
    [SerializeField] private float despawnYPosition;

    private bool isFalling = true;

    private float timer;

    [SerializeField] private GameObject boem;
    [SerializeField] private float explosionCooldown;
    private float explosionTimer;

    private bool hasEnteredScene = false;
    [SerializeField] private float enterTime;
    [SerializeField] private float enterPos;

    [SerializeField] private float pointsAddedEveryHit;
    [SerializeField] private float pointsAddedAfterDeath;
    [SerializeField] private GameObject scoreManagerTemp;
    private scoreManager scoreManager;
    
    [SerializeField] private List<GameObject> waves;
    private bool wavesCleared = false;
    private bool hasBossWaveStarted = false;

    [SerializeField] private string transitionToLevel;
    [SerializeField] private float transitionToLevelTimer;
    private float transitionToLevelTimerTimer;

    private void Start()
    {
        transitionToLevelTimerTimer = transitionToLevelTimer;
        transform.position = new Vector3(1000, 1000, 1000);
        scoreManager = scoreManagerTemp.transform.GetChild(0).GetComponent<scoreManager>();
    }

    private void FixedUpdate()
    {
        timer++;
        shootingTimer--;
        hitTimer--;
        enemyTimer--;

        if (lives > 0 && hasEnteredScene)
        {
            for (int i = 0; i < enemiesSpawn.Count; i++)
            {
                if (Random.Range(0, enemySpawnChance[i]) == 1 && enemyTimer < 0)
                {
                    GameObject enemy = Instantiate(enemiesSpawn[i]);
                    enemy.GetComponent<enemy_movement>().startPos = transform.position;
                    enemyTimer = enemyCooldown;
                }
            }

            transform.position = new Vector3(startPos.x, Mathf.Sin(timer / 50 * speed) * 3 + startPos.y, transform.position.z);
            if (Random.Range(0, shootingSpeed) == 1 && shootingTimer < 0)
            {
                for (int i = 0; i < bulletSummon.Count; i++)
                {
                    Instantiate(bullet, new Vector3(transform.position.x + bulletSummon[i].x, transform.position.y + bulletSummon[i].y, 0), Quaternion.identity);
                }
                shootingTimer = shootingCooldown;
            }
        }

        if (hitTimer < 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }

        if(lives < 1)
        {
            GetComponent<Rigidbody>().useGravity = isFalling;
            GetComponent<Rigidbody>().isKinematic = false;
            if(isFalling)
            {
                isFalling = false;
            } else
            {
                isFalling = true;
            }

            explosionTimer--;

            if(explosionTimer < 0)
            {
                explosionTimer = explosionCooldown;
                Instantiate(boem, transform.position, Quaternion.identity);
            }
            if (transform.position.y > despawnYPosition)
            {
                scoreManager.score += pointsAddedAfterDeath;
            }
        }

        if (transform.position.y < despawnYPosition)
        {
            transitionToLevelTimerTimer--;
            if(transitionToLevelTimerTimer < 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene(transitionToLevel);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerBullet" && hitTimer < 0)
        {
            Destroy(other.gameObject);
            lives--;
            GetComponent<MeshRenderer>().enabled = false;
            hitTimer = invisibiltyAfterHit;
            scoreManager.score += pointsAddedEveryHit;
        }
    }

    private IEnumerator enter()
    {
        hasBossWaveStarted = true;
        transform.position = new Vector3(startPos.x, startPos.y, enterPos);
        for (int i = 0; i < enterTime; i++)
        {
            transform.position -= new Vector3(0, 0, enterPos/enterTime);
            yield return new WaitForSeconds(0.02f);
        }
        transform.position = startPos;
        hasEnteredScene = true;
    }

    private void Update()
    {
        if(!hasBossWaveStarted)
        {
            wavesCleared = true;
            for (int i = 0; i < waves.Count; i++)
            {
                for (int j = 0; j < waves[i].transform.childCount; j++)
                {
                    if (waves[i].transform.GetChild(j) != null)
                    {
                        wavesCleared = false;
                    }
                }
            }

            if (wavesCleared)
            {
                StartCoroutine(enter());
            }
        }
    }
}
