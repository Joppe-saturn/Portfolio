using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class bulletMove : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;

    public float timeBeforeDespawn;

    private GM gameManager;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GM>();
    }

    private void Update()
    {
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
    }

    private void FixedUpdate()
    {
        timeBeforeDespawn--;
        rb.velocity = transform.forward * speed;
        if(timeBeforeDespawn < 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "astroid2")
        {
            transform.position = other.gameObject.transform.parent.transform.parent.transform.position;
        }

        if(other.gameObject.tag == "ufo")
        {
            gameManager.scoreUpdate(10);
            Destroy(gameObject);
        }
    }
}
