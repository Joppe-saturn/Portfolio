using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratMove : MonoBehaviour
{
    [SerializeField] private float timeToDie;
    [SerializeField] private float ratSpeed;
    private float timer;
    [SerializeField] private GameObject bomb;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeToDie) 
        { 
            gameObject.SetActive(false);
        }
        transform.position = new Vector3(transform.position.x + ratSpeed * transform.rotation.y, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform == bomb)
        {
            gameObject.SetActive(false);
        }
    }
}
