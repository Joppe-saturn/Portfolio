using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject boem;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject bonus;
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private GameObject extraLive;
    [SerializeField] private AudioSource extrLiveSound;

    public float speed;
    public float rotationSpeed;
    private Rigidbody rb;

    public float buttonCooldownAmount;
    private float buttonCooldown;

    public int lives;

    private bool invinsibility = false;
    private float invinsibilityTimer;
    private float invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne;

    [SerializeField] private float scoreBeforeBonus;
    [SerializeField] private float bonusScore;
    private GM gameManager;
    private astroidSpawner astroidSpawn;

    private bool hasActivatedBonus = false;

    private float dashTimer;
    [SerializeField] private float dashSpeed;

    [SerializeField] private float timeBeforeExtraLives;
    private float extraLivesTimeIncrease;
    private bool hasCollectedLive = true;

    private string shipLivesText;

    public bool isMoving = false;

    private float flikker = 0;

    [SerializeField] private string currentPowerup;

    private float powerupTimer;

    [SerializeField] private GameObject powerup;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private List<Material> powerupMaterials;

    private void Start()
    {
        lives = 3;
        rb = GameObject.FindAnyObjectByType<Rigidbody>();
        gameManager = FindObjectOfType<GM>();
        astroidSpawn = FindObjectOfType<astroidSpawner>();
        extraLivesTimeIncrease = timeBeforeExtraLives;
        invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne = 35;
        invinsibility = true;
    }

    private void Update()
    {
        for(int i = 0; i < lives; i++)
        {
            shipLivesText = shipLivesText + "A";
        }
        livesText.text = "Lives: " + shipLivesText;
        shipLivesText = "";

        if ((gameManager.score - scoreBeforeBonus) > 0 && !hasActivatedBonus)
        {
            GameObject newBonus = Instantiate(bonus);
            newBonus.transform.position = new Vector3(Random.Range(-24.9f, 24.9f), 0, Random.Range(-12.6f, 12.6f));
            hasActivatedBonus = true;
        }

        if (lives < 1)
        {
            gameOverScreen.SetActive(true);
            Destroy(gameObject);
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && buttonCooldown < 0)
        {
            if(currentPowerup == "multi shot")
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 12, transform.rotation.eulerAngles.z));
                Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 12, transform.rotation.eulerAngles.z));
                buttonCooldown = buttonCooldownAmount;
            } else if (currentPowerup == "no cooldown")
            {
                Instantiate(bullet, transform.position, transform.rotation);
                buttonCooldown = -1;
            }
            else if (currentPowerup == "I have your back")
            {
                Instantiate(bullet, transform.position, transform.rotation);
                Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z));
                buttonCooldown = buttonCooldownAmount;
            }
            else if (currentPowerup == "nuke")
            {
                for(int i = 0; i < 360; i += 2)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + i, transform.rotation.eulerAngles.z));
                }
                buttonCooldown = buttonCooldownAmount * 50;
                powerupTimer = -1;
            }
            else
            {
                Instantiate(bullet, transform.position, transform.rotation);
                buttonCooldown = buttonCooldownAmount;
                if(Random.Range(0,200) == 69)
                {
                    GameObject powerUUp = Instantiate(powerup);
                    powerUUp.transform.position = new Vector3(Random.Range(-17.0f, 17.0f), 0, Random.Range(-9.0f, 9.0f));
                }
            }
        }

        if (transform.position.z > 11.1)
        {
            transform.position += new Vector3(0, 0, -22.2f);
        }
        if (transform.position.z < -11.1)
        {
            transform.position += new Vector3(0, 0, 22.2f);
        }
        if (transform.position.x > 20.2)
        {
            transform.position += new Vector3(-40.4f, 0, 0);
        }
        if (transform.position.x < -20.2)
        {
            transform.position += new Vector3(40.4f, 0, 0);
        }

        if (Input.anyKeyDown && invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne < 0 && invinsibility)
        {
            transform.position = Vector3.zero;
            invinsibility = false;
        }

        if (invinsibility)
        {
            invinsibilityTimer++;
            if (invinsibilityTimer > 25)
            {
                flikker = flikker - flikker * 2 + 1;
                transform.position = new Vector3(0, 0, flikker * 1000);
                invinsibilityTimer = 0;
            }
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) && dashTimer < 0 && gameManager.score >= 200)
        {
            dashTimer = 150;
            rb.AddForce(transform.forward * dashSpeed);
            gameManager.scoreUpdate(-200);
        }

        if ((gameManager.score - timeBeforeExtraLives) > 0 && hasCollectedLive)
        {
            GameObject newLive = Instantiate(extraLive);
            newLive.transform.position = new Vector3(Random.Range(-24.9f, 24.9f), 0, Random.Range(-12.6f, 12.6f));
            hasCollectedLive = false;
            timeBeforeExtraLives += extraLivesTimeIncrease;
        }
    }

    private void FixedUpdate()
    {
        buttonCooldown--;
        
        Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical") * speed);
        
        if(move.z < 0)
        {
            move.z = 0;
        }
        
        if (move.z > 0) 
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }

        rb.AddRelativeForce(move);
        rb.rotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Horizontal") * rotationSpeed, Vector3.up);

        invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne--;

        dashTimer--;

        powerupTimer--;

        if (powerupTimer < 0)
        {
            currentPowerup = "no";
            powerupIndicator.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "astroid" && !invinsibility)
        {
            loseSound.Play();
            Instantiate(boem, transform.position, transform.rotation);
            lives--;
            transform.position = Vector3.zero;
            invinsibility = true;
            rb.velocity = Vector3.zero;
            buttonCooldown = 50;
            invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne = 35;
            astroidSpawn.changeAmountAStroid(Mathf.Floor((astroidSpawn.maxAstroids / 3) * 1.5f - 0.5f) * -1);
        }
        
        if(other.gameObject.tag == "scoreUp")
        {
            gameManager.scoreUpdate(bonusScore);
            other.transform.position = new Vector3(Random.Range(-24.9f, 24.9f), 0, Random.Range(-12.6f, 12.6f));
        }

        if(other.gameObject.tag == "Live")
        {
            extrLiveSound.Play();
            hasCollectedLive = true;
            lives++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "powerUp")
        {
            powerupIndicator.SetActive(true);
            float tempNumber = Random.Range(0, 7);
            if (tempNumber == 0 || tempNumber == 1)
            {
                powerupTimer = 1500;
                currentPowerup = "multi shot";
                powerupIndicator.GetComponent<MeshRenderer>().material = powerupMaterials[0];
            }
            if (tempNumber == 2 || tempNumber == 3)
            {
                powerupTimer = 1500;
                currentPowerup = "no cooldown";
                powerupIndicator.GetComponent<MeshRenderer>().material = powerupMaterials[1];
            }
            if (tempNumber == 4 || tempNumber == 5)
            {
                powerupTimer = 1500;
                currentPowerup = "I have your back";
                powerupIndicator.GetComponent<MeshRenderer>().material = powerupMaterials[2];
            }
            if (tempNumber == 6)
            {
                powerupTimer = 150000;
                currentPowerup = "nuke";
                powerupIndicator.GetComponent<MeshRenderer>().material = powerupMaterials[3];
            }
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "ufo" && !invinsibility)
        {
            loseSound.Play();
            Instantiate(boem, transform.position, transform.rotation);
            lives--;
            transform.position = Vector3.zero;
            invinsibility = true;
            rb.velocity = Vector3.zero;
            buttonCooldown = 50;
            invinsibilityTimerButForTheActualInvisibilityNotForTheVisualsLikeTheOtherOne = 35;

            Instantiate(boem, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
    }
}
