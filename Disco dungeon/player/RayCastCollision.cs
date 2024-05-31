using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCollision : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 startPos;
    public string direction;

    private Playermovement player;

    private bool isDoneChecking = false;

    void Start()
    {
        player = FindAnyObjectByType<Playermovement>();
        startPos = transform.position;
        StartCoroutine(move());
    }



    private IEnumerator move()
    {
        if (direction == "x")
        {
            while (transform.position.x < startPos.x + 1)
            {
                GetComponent<Rigidbody>().velocity += new Vector3(speed, 0, 0);
                yield return null;
            }
            isDoneChecking = true;
        }
        if (direction == "-x")
        {
            while (transform.position.x > startPos.x - 1)
            {
                GetComponent<Rigidbody>().velocity -= new Vector3(speed, 0, 0);
                yield return null;
            }
            isDoneChecking = true;
        }
        if (direction == "z")
        {
            while (transform.position.z < startPos.z + 1)
            {
                GetComponent<Rigidbody>().velocity += new Vector3(0, 0, speed);
                yield return null;
            }
            isDoneChecking = true;
        }
        if (direction == "-z")
        {
            while (transform.position.z > startPos.z - 1)
            {
                GetComponent<Rigidbody>().velocity -= new Vector3(0, 0, speed);
                yield return null;
            }
            isDoneChecking = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(isDoneChecking)
        {
            if (direction == "x")
            {
                player.moveForward(new Vector3(1, 0, 0));
                Destroy(gameObject);
            }
            if (direction == "-x")
            {
                player.moveForward(new Vector3(-1, 0, 0));
                Destroy(gameObject);
            }
            if (direction == "z")
            {
                player.moveForward(new Vector3(0, 0, 1));
                Destroy(gameObject);
            }
            if (direction == "-z")
            {
                player.moveForward(new Vector3(0, 0, -1));
                Destroy(gameObject);
            }
        }
    }
}
