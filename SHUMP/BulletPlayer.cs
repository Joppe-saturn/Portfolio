using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Hp = 1;

    void Awake()
    {
        Destroy(gameObject, Hp);
    }

    void OnCollisionEnter(Collision collision)
    {
        /*Destroy(collision.gameObject);
        Destroy(gameObject);*/
    }
}