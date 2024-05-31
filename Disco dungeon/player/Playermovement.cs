using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [SerializeField] private float playerInputWindow;

    [SerializeField] private GameObject ray;

    public bool canMove = false;

    private float timer;

    private List<float> levelBpm = new List<float>();
    private int level;

    [SerializeField] private Gun gun;
    [SerializeField] private float energyExponent;
    private float potentioalEnergy;

    private Vector3 startPos;
    private GameObject playerSpawnObject;

    [SerializeField] private bool isImmortal;
    [SerializeField] private bool canShootInfinitly;

    private void Awake()
    {
        playerSpawnObject = FindAnyObjectByType<PlayerSpawn>().gameObject;
        startPos = new Vector3(playerSpawnObject.transform.position.x, 0.5f, playerSpawnObject.transform.position.z);
        transform.position = startPos;
    }

    private void Start()
    {
        levelBpm = LevelBuilder.instance.levelbpm;
        level = LevelBuilder.instance.level;
    }

    private void Update()
    {
        if ((timer < playerInputWindow || timer > 50 / (levelBpm[level] / 60) - playerInputWindow) && LevelBuilder.instance.hasGameStartedYet)
        {
            if (timer < playerInputWindow)
            {
                potentioalEnergy = (playerInputWindow - timer) * energyExponent;
            }

            else
            {
                potentioalEnergy = (timer - (50 / (levelBpm[level] / 60) - playerInputWindow)) * energyExponent;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && canMove)
            {
                GameObject bullet = Instantiate(ray, transform.position, Quaternion.identity);
                bullet.GetComponent<RayCastCollision>().direction = "-x";
                canMove = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && canMove)
            {
                GameObject bullet = Instantiate(ray, transform.position, Quaternion.identity);
                bullet.GetComponent<RayCastCollision>().direction = "x";
                canMove = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && canMove)
            {
                GameObject bullet = Instantiate(ray, transform.position, Quaternion.identity);
                bullet.GetComponent<RayCastCollision>().direction = "z";
                canMove = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove)
            {
                GameObject bullet = Instantiate(ray, transform.position, Quaternion.identity);
                bullet.GetComponent<RayCastCollision>().direction = "-z";
                canMove = false;
            }
            if (Input.GetMouseButton(0))
            {
                gun.shoot();
                canMove = false;
            }
        } else
        {
            canMove = true;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gun.SubEnergy();
            }
        }

        if(Input.GetKey(KeyCode.Space) && canShootInfinitly)
        {
            gun.energy = 3;
        }
    }

    private void FixedUpdate()
    {
        timer++;
        
        if(timer > 50 /(levelBpm[level] / 60))
        {
            GetComponent<Animator>().SetBool("Beat", true);
            timer = 0;
        } else
        {
            GetComponent<Animator>().SetBool("Beat", false);
        }
    }

    public void moveForward(Vector3 direction)
    {
        transform.position += direction;
        gun.AddEnergy(potentioalEnergy);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !isImmortal)
        {
            StartCoroutine(restartLevel());
        }
        if (other.tag == "Tresure")
        {
            other.GetComponent<Tresure>().GetCollected();
        }
    }

    private IEnumerator restartLevel()
    {
        gun.energy = 0;
        transform.position = startPos;
        LevelBuilder.instance.fallMultiplier = 0;
        LevelBuilder.instance.clearScreen = true;
        yield return null;
        LevelBuilder.instance.buildScreen = true;
    }

    public IEnumerator Winner()
    {
        while (true)
        {
            transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
