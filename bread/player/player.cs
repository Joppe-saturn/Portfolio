using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;

    [SerializeField] private float playerBaseSpeed;
    [SerializeField] private float playerBaseRotation;
    public GameObject boom;
    public GameObject fire;
    public GameObject checkpointSparkle;

    private float extraRotation = 0;
    public float startEnergy;
    public float energy = 0;
    
    public bool isPaused = false;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private levelTransistion currentLevel;
    [SerializeField] private GameObject pressSpaceText;
    [SerializeField] private GameObject[] energyZone;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private GameObject checkPointBread;
    [SerializeField] private Transform triggerSquare;
    [SerializeField] private Transform bomb;

    private bool playerKnowsHowToPressTheSpaceButton = true;

    private float pressSpaceTextTimer;
    private float textVisibility = 1;

    public bool hasCheckPointBeenReached = false;
    public bool hasDied = false;

    public bool triggerSquareActive = false;

    private void Start()
    {
        Debug.Log(currentLevel.transistionToLevel - 2);
        m_Rigidbody = GetComponent<Rigidbody2D>();
        energyReset();
    }

    private void Update()
    {
        hasDied = false;
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            if(isPaused)
            {
                isPaused = false;
            } else
            {
                isPaused = true;
            }
        }
        if (currentLevel.transistionToLevel - 2 == 2 && playerKnowsHowToPressTheSpaceButton)
        {
            isPaused = true;
            if (Input.GetKey(KeyCode.Space)) {
                playerKnowsHowToPressTheSpaceButton = false;
                isPaused = false;
                pressSpaceText.SetActive(false);
            }
            if (pressSpaceTextTimer > 0.5)
            {
                pressSpaceText.SetActive(false);
                textVisibility = -1;
            } else if (pressSpaceTextTimer < 0)
            {
                pressSpaceText.SetActive(true);
                textVisibility = 1;
            }
            pressSpaceTextTimer += Time.deltaTime * textVisibility;
            pauseMenu.SetActive(false);
            return;
        }
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            return;
        } else
        {
            pauseMenu.SetActive(false);
        }
        m_Rigidbody.rotation += ((playerBaseRotation + (extraRotation / 10)) * 1f * (Time.deltaTime * 100)) * 2;
        if(Input.GetKey(KeyCode.Space) && energy > 0) 
        {
            extraRotation = playerBaseRotation * -15;
            energy -= Time.deltaTime;
        } else
        {
            extraRotation = 0;
        }
        if(energy < 0)
        {
            energy = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            m_Rigidbody.AddForce(new Vector2(playerBaseSpeed, 0));
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            m_Rigidbody.AddForce(new Vector2(-playerBaseSpeed, 0));
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            m_Rigidbody.AddForce(new Vector2(0, playerBaseSpeed));
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            m_Rigidbody.AddForce(new Vector2(0, -playerBaseSpeed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < energyZone.Length; i++)
        {
            if (collision.gameObject == energyZone[i])
            {
                energyReset();
                energyZone[i].SetActive(false);
                energyZone[i].SetActive(true);
                return;
            }
        }
        if (currentLevel.transistionToLevel - 2 == 3)
        {
            die();
        }
        if (collision.transform == checkPoint)
        {
            float tempX = checkPoint.position.x;
            float tempY = checkPoint.position.y;
            spawnPoint.transform.position = new Vector3(tempX, tempY, 0);
            if(!hasCheckPointBeenReached)
            {
                Instantiate(checkpointSparkle, checkPoint.transform.position, transform.rotation);
            }
            hasCheckPointBeenReached = true;
        } else if(collision.transform == triggerSquare)
        {
            triggerSquareActive = true;
            Debug.Log(triggerSquareActive);
        }
        else if(collision.gameObject.tag != bomb.gameObject.tag)
        {
            die();
        } 
    }

    private void energyReset()
    {
        energy = startEnergy;
    }

    public void NextLevel()
    {
        energyReset();
        float tempX = spawnPoint.position.x;
        float tempY = spawnPoint.position.y;
        transform.position = new Vector3(tempX, tempY, 0);
    }

    private void die()
    {
        for (int timesPaeticle = 0; timesPaeticle < 5; timesPaeticle++)
        {
            Instantiate(boom, transform.position, transform.rotation);
            Instantiate(fire, transform.position, transform.rotation);
        }
        float tempX = spawnPoint.position.x;
        float tempY = spawnPoint.position.y;
        transform.position = new Vector3(tempX, tempY, 0);
        energyReset();
        if (hasCheckPointBeenReached)
        {
            GameObject deadBread = Instantiate(checkPointBread, transform.position, transform.rotation);
            deadBread.transform.position = new Vector3(tempX + UnityEngine.Random.Range(-2, 2), tempY + UnityEngine.Random.Range(-15, 15) / 10, 0);
        }
        hasDied = true;
    }
}
