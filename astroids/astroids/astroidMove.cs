using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class astroidMove : MonoBehaviour
{
    [SerializeField] private GameObject boem;

    [SerializeField] private float timerTillDie;
    public float speed;

    private Rigidbody rb;

    public int level;
    
    private GM gameManager;

    private float size;

    private bool hasWrap = false;

    private boss bossObject;

    private void Start()
    {
        gameManager = FindObjectOfType<GM>();
        bossObject = FindObjectOfType<boss>();
        if (gameObject.name != "1" && gameObject.name != "2" && gameObject.name != "3")
        {
            level = 3;
            size = Random.Range(25000, 35000) / 10000f;
            transform.localScale = new Vector3(size, size, size);
        }
        else
        {
            level = int.Parse(gameObject.name);
        }
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * (1 / transform.localScale.x) * speed);
    }

    private void Update()
    {
        if (transform.position.z > 11.1 && hasWrap)
        {
            transform.position += new Vector3(0, 0, -22.2f);
        }
        if (transform.position.z < -11.1 && hasWrap)
        {
            transform.position += new Vector3(0, 0, 22.2f);
        }
        if (transform.position.x > 20.2 && hasWrap)
        {
            transform.position += new Vector3(-40.4f, 0, 0);
        }
        if (transform.position.x < -20.2 && hasWrap)
        {
            transform.position += new Vector3(40.4f, 0, 0);
        }
        
        if(transform.position.x > -17.7 && transform.position.x < 17.7 && transform.position.z > -8.6 && transform.position.z < 8.6 && !hasWrap)
        {
            hasWrap = true;
        }

        if(!hasWrap)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
            }
        } else
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        if(bossObject.isBossActive)
        {
            for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
            {
                Instantiate(boem, transform.GetChild(0).transform.GetChild(i).transform.position, Quaternion.identity);
            }
            Instantiate(boem, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            gameManager.scoreUpdate(100);
            level--;
            
            Instantiate(boem, transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            for(int i = 0; i < transform.GetChild(0).transform.childCount; i++) 
            {
                Instantiate(boem, transform.GetChild(0).transform.GetChild(i).transform.position, Quaternion.identity);
            }

            if (level <= 0)
            {
                Destroy(gameObject);
            } else
            {
                gameObject.transform.localScale /= 2;
                transform.rotation = Quaternion.EulerAngles(0, Random.Range(0f, 360f), 0);
                transform.GetChild(0).gameObject.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
                GameObject newAstorid = Instantiate(gameObject);
                newAstorid.name = level.ToString();
                transform.rotation = Quaternion.EulerAngles(0, Random.Range(0f, 360f), 0);
                rb.AddForce(transform.forward * (1 / transform.localScale.x) * speed);
                transform.rotation = Quaternion.EulerAngles(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            }
        }
    }

    private void FixedUpdate()
    {
        timerTillDie -= 1;
        if(timerTillDie < 0 && !hasWrap)
        {
            Destroy(gameObject);
        }
    }
}
