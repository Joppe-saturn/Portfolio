using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    [SerializeField] private string movementType;

    [SerializeField] private float speed;
    [SerializeField] private Vector2 enemyMovementRange;

    [SerializeField] public Vector3 startPos;
    [SerializeField] private float AppearAfter;
    private bool HasEnteredScene = false;

    [SerializeField] private GameObject bullet;

    [SerializeField] private int shootingSpeed;
    [SerializeField] private bool canShoot;
    [SerializeField] private float timeBetweenBulletSpawn;
    private float bulletCooldown;

    [SerializeField] private float lives;

    private float timer;

    [SerializeField] private bool noIntro;
    [SerializeField] private bool canDive;
    [SerializeField] private string diveType;
    [SerializeField] private float diveSpeed;
    [SerializeField] private float retriveSpeed;
    [SerializeField] private float diveDistance;
    [SerializeField] private int diveFrequency;
    [SerializeField] private float timeBetweenDive;
    private float diveCooldown;
    private bool isDiving = true;
    private bool isStartingToDive = false;
    private Vector3 retrivePos;
    [SerializeField] private float WaitingTimeBeforeDiving;
    private float diveWaitTimer;

    [SerializeField] private bool canShootWhileDiving;
    [SerializeField] private bool canShootWhileRetriving;
    [SerializeField] private int shootFrequencyWhileDiving;
    [SerializeField] private int shootFrequencyWhileRetriving;

    [SerializeField] private bool kamikazie;
    [SerializeField] private bool canShootWhileKamikazie;
    [SerializeField] private bool canShootWhileKamikazieFail;
    [SerializeField] private float kamikazieAtWhatHp;
    [SerializeField] private float kamikazieDistance;
    [SerializeField] private float kamikazieSpeed;
    [SerializeField] private float failedKamikazieSpeed;
    [SerializeField] private int shootingSpeedWhileKamikazie;
    [SerializeField] private int shootingSpeedWhileKamikazieFailed;
    [SerializeField] private bool dieAfterKamikazie;
    [SerializeField] private float dieAfterKamikazieAttempts;
    private bool isDoingAKamikazie = false;
    private bool isReturningAfterKamikazie = false;
    [SerializeField] private bool explodeAfterDeath;
    [SerializeField] private GameObject boem;

    private scoreManager scoreManager;
    [SerializeField] private float scoreAddedByhit;
    [SerializeField] private float scoreAdedByDeath;
    private bool kamikazieDeath = false;
    [SerializeField] private float invisibilityAfterHit;
    private float hitTimer;

    void Start()
    {
        diveWaitTimer = WaitingTimeBeforeDiving;
        scoreManager = FindObjectOfType<scoreManager>();
        retrivePos = startPos;
        transform.position = new Vector3(12, startPos.y - 60, 0);
    }

    private void FixedUpdate()
    {
        hitTimer--;
        Debug.Log((!isDiving || !isStartingToDive));
        if ((!isDiving && !isStartingToDive) && !isDoingAKamikazie)
        {
            timer++;
            bulletCooldown--;
            diveCooldown--;

            retrivePos = transform.position;

            if (movementType == "ship1")
            {
                transform.position = new Vector3(startPos.x, Mathf.Sin(timer / 50 * speed) * enemyMovementRange.y + startPos.y, transform.position.z);
            }

            if (movementType == "ship2")
            {
                transform.position = new Vector3(Mathf.Cos(timer / 50 * speed) * enemyMovementRange.x + (startPos.x - enemyMovementRange.x), Mathf.Sin(timer / 50 * speed) * enemyMovementRange.y + startPos.y, transform.position.z);
            }

            if (movementType == "ship3")
            {
                transform.position = new Vector3(Mathf.Cos(-timer / 500 * speed) * enemyMovementRange.x + (startPos.x - enemyMovementRange.x), Mathf.Sin(timer / 50 * speed) * enemyMovementRange.y + startPos.y, 0);
            }

            if (canShoot)
            {
                if (Random.Range(0, shootingSpeed) == 1 && bulletCooldown < 0)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    bulletCooldown = timeBetweenBulletSpawn;
                }
            }
        }

        if (canDive && !isDiving)
        {
            if ((Random.Range(0, diveFrequency) == 1 && diveCooldown < 0) || isStartingToDive)
            {
                diveWaitTimer--;
                isStartingToDive = true;
                if (diveWaitTimer < 0)
                {
                    isDiving = true;
                    isStartingToDive = false;
                    diveCooldown = timeBetweenDive;
                    StartCoroutine(movementStart(diveType));
                }
            }
        }

        if (lives < 1)
        {
            Destroy(gameObject);
            if (!kamikazieDeath)
            {
                scoreManager.score += scoreAdedByDeath;
            }

            if (explodeAfterDeath)
            {
                Instantiate(boem, transform.position, transform.rotation);
            }
        }

        if (kamikazie && lives <= kamikazieAtWhatHp && !isDiving && !isDoingAKamikazie)
        {
            canShootWhileDiving = canShootWhileKamikazie;
            canShootWhileRetriving = canShootWhileKamikazieFail;
            diveSpeed = kamikazieSpeed;
            retriveSpeed = failedKamikazieSpeed;
            diveDistance = kamikazieDistance;
            shootFrequencyWhileDiving = shootingSpeedWhileKamikazie;
            shootFrequencyWhileRetriving = shootingSpeedWhileKamikazieFailed;
            isDiving = true;
            isDoingAKamikazie = true;
            StartCoroutine(movementStart(diveType));
        }

        if (dieAfterKamikazie && isReturningAfterKamikazie)
        {
            dieAfterKamikazieAttempts--;
            if (dieAfterKamikazieAttempts < 0)
            {
                lives = 0;
                kamikazieDeath = true;
            }
            isReturningAfterKamikazie = false;
        }

        AppearAfter -= 0.02f;
        if (isDiving && AppearAfter < 0 && !HasEnteredScene)
        {
            HasEnteredScene = true;
            transform.position = new Vector3(12, startPos.y, 0);
            if (!noIntro)
            {
                StartCoroutine(movementStart(diveType));
            }
            else
            {
                isDiving = false;
                isDoingAKamikazie = false;
            }
        }

        if (hitTimer < 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private IEnumerator movementStart(string typeOfShip)
    {
        if (typeOfShip == "ship1")
        {
            while (transform.position.x > diveDistance)
            {
                transform.position += new Vector3((-9 - transform.position.x) / 25 * diveSpeed, 0, 0);
                if (Random.Range(0, shootFrequencyWhileDiving) == 1 && canShootWhileDiving)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.02f);
            }
            if (dieAfterKamikazie && isDoingAKamikazie)
            {
                isReturningAfterKamikazie = true;
            }
            while (transform.position.x < retrivePos.x)
            {
                timer++;
                transform.position = new Vector3(transform.position.x + retriveSpeed / 4, Mathf.Sin(timer / 50 * speed) * enemyMovementRange.y + retrivePos.y, 0);
                if (Random.Range(0, shootFrequencyWhileRetriving) == 1 && canShootWhileRetriving)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.02f);
            }
            isDiving = false;
            isDoingAKamikazie = false;
            diveWaitTimer = WaitingTimeBeforeDiving;
        }

        if (typeOfShip == "ship2")
        {
            while (transform.position.x > diveDistance)
            {
                transform.position += new Vector3((-9 - transform.position.x) / 20 * diveSpeed, 0, 0);
                if (Random.Range(0, shootFrequencyWhileDiving) == 1 && canShootWhileDiving)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.02f);
            }
            if (dieAfterKamikazie && isDoingAKamikazie)
            {
                isReturningAfterKamikazie = true;
            }
            while (transform.position.x < retrivePos.x)
            {
                transform.position += new Vector3(retriveSpeed / 10, 0, 0);
                if (Random.Range(0, shootFrequencyWhileRetriving) == 1 && canShootWhileRetriving)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.02f);
            }
            isDiving = false;
            isDoingAKamikazie = false;
        }

        if (typeOfShip == "no")
        {
            while (transform.position.x > retrivePos.x)
            {
                transform.position += new Vector3((-9 - transform.position.x) / 50, 0, 0);
                if (Random.Range(0, shootFrequencyWhileDiving) == 1 && canShootWhileDiving)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.02f);
            }
            isDiving = false;
            isDoingAKamikazie = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerBullet" && HasEnteredScene && hitTimer < 0)
        {
            Destroy(other.gameObject);
            lives--;
            scoreManager.score += scoreAddedByhit;
            GetComponent<MeshRenderer>().enabled = false;
            hitTimer = invisibilityAfterHit;
            if (lives < 1)
            {
                kamikazieDeath = false;
            }
        }

        if (other.tag == "player")
        {
            lives--;
            if (lives < 1)
            {
                kamikazieDeath = true;
            }
        }
    }
}
